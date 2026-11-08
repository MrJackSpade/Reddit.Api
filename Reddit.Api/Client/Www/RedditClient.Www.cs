using Reddit.Api.Exceptions;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Reddit.Api.Client
{
    /// <summary>
    /// The calls sent to www.reddit.com with the browser session cookies, in this file and the
    /// Reads and Writes partials next to it. This is where calls should end up, for parity with what
    /// Reddit's main site serves. The wider story is on the OAuth partial.
    ///
    /// CONTEXT: why almost everything moved here on 2026-11-06.
    /// Reddit is closing the public API: access starts closing on 2027-01-12 for apps and users that
    /// haven't registered, and closes for everyone in March 2027. Deaddit won't register or move to
    /// Devvit, which can't host a client, so it is moving to the website ahead of that, and may end up
    /// as little more than a custom browser.
    ///
    /// How the website differs from OAuth:
    /// - Authentication is the session cookies, plus the session's modhash on anything but a GET. The
    ///   send plumbing adds both, so calls here only pass <see cref="RequestRoute.Www"/>.
    /// - JSON needs a .json path suffix, which the plumbing also adds.
    /// - Blocks are per path. Listing rules live in <see cref="RouteListing"/>:
    ///   /hot.json is blocked but the same listing with no sort is the hot listing, and /r/all/*
    ///   only answers on old.reddit.com. "new" and "rising" are blocked as JSON except on multis, so
    ///   <see cref="GetSortedPostsAsync"/> reads them from the /svc/shreddit feed partials www's own
    ///   pages page through (24 posts a page; the home feed's first page is 3), which render each
    ///   post in full, so a page is one request. (Until 2026-11-08 it fetched each post as JSON, one
    ///   request per post: slow, and those 403 too.) Checked against old.reddit.com: the same posts
    ///   in the same order, except the front page's "rising", where www shows its own selection.
    /// - Search reads the matching IDs from www's search partial the same way (see the Reads partial).
    /// - The partials are asked for as partials (<see cref="GetPartialAsync"/>); otherwise www wraps
    ///   them in a page shell many times the size.
    ///
    /// Posts by ID: /api/info and /by_id were blocked on 2026-11-06, so a post is fetched through its
    /// thread, one request per post. Comments can't be fetched by ID alone any more: every thread URL
    /// needs the post ID (/comments/{post}/_/{comment}.json), and nothing else returns a lone comment.
    ///
    /// Threads (2026-11-08): comments, "load more" and translations come from www's comments
    /// partials as HTML (ShredditCommentParser), which carry the user's reply state and, with tl=,
    /// translations, which the JSON doesn't. The thread page itself is behind a JavaScript challenge;
    /// the app loads it in a WebView when it needs a post from it (ShredditThreadParser).
    /// </summary>
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<Listing<Thing<T>>?> GetListingAsync<T>(string endpoint, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            (string path, RequestRoute route) = RouteListing(endpoint);

            return await this.GetAsync<Listing<Thing<T>>>($"{path}{parameters?.ToQueryString() ?? string.Empty}", cancellationToken, route);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetSortedPostsAsync(string path, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            string fullPath = $"/{path.Trim('/')}";

            if (IsMulti(fullPath))
            {
                await this.TryAuthenticateAsync(cancellationToken);
                return await this.GetAsync<Listing<Thing<Link>>>($"{fullPath}{parameters?.ToQueryString() ?? string.Empty}", cancellationToken, RequestRoute.Www);
            }

            if (IsAll(fullPath))
            {
                return await this.GetPostsFromListingHtmlAsync(fullPath, parameters, cancellationToken);
            }

            await this.EnsureWebSessionAsync("This sort", cancellationToken);

            string html = await this.GetPartialAsync(FeedPartial(fullPath, parameters?.After), cancellationToken);
            (List<Thing<Link>> posts, string? after) = ShredditListingParser.ParseFeed(html);

            return new Listing<Thing<Link>> { Data = new ListingData<Thing<Link>> { Children = posts, After = after } };
        }

        /// <summary>
        /// The www partial that serves a "new" or "rising" listing, the way www's own pages page
        /// through it: the subreddit feed, or the home feed for the front page.
        /// </summary>
        private static string FeedPartial(string path, string? after)
        {
            Match match = Regex.Match(path, "^/(?:r/(?<subreddit>[^/]+)/)?(?<sort>new|rising)$");

            if (!match.Success)
            {
                throw new InvalidOperationException($"www has no feed partial for {path}.");
            }

            string sort = match.Groups["sort"].Value;
            string afterParameter = after == null ? string.Empty : $"&after={Uri.EscapeDataString(ShredditListingParser.EncodeAfter(after))}";

            return match.Groups["subreddit"].Success
                ? $"/svc/shreddit/community-more-posts/{sort}/?name={Uri.EscapeDataString(match.Groups["subreddit"].Value)}{afterParameter}"
                : $"/svc/shreddit/feeds/home-feed?sort={sort.ToUpperInvariant()}{afterParameter}";
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetByIdAsync(IEnumerable<string> fullnames, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            List<Thing<Link>> posts = [];

            foreach (string fullname in fullnames)
            {
                Thing<Link>? post = await this.GetPostAsync(fullname, cancellationToken);

                // /by_id left out posts it could not return, and callers expect that.
                if (post != null)
                {
                    posts.Add(post);
                }
            }

            return new Listing<Thing<Link>> { Data = new ListingData<Thing<Link>> { Children = posts } };
        }

        /// <inheritdoc />
        public async Task<(Thing<Link>? Post, Listing<Thing<Comment>>? Comments)> GetCommentsAsync(
            string articleId,
            string? commentId = null,
            string? sort = null,
            int? limit = null,
            int? depth = null,
            int? context = null,
            CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            string query = new QueryStringBuilder()
                .Add("comment", commentId)
                .Add("sort", sort)
                .Add("limit", limit)
                .Add("depth", depth)
                .Add("context", context)
                .Build();

            string endpoint = $"/comments/{articleId}{query}";

            // Reddit returns an array of two listings: [post, comments]
            JsonElement result = await this.GetAsync<JsonElement>(endpoint, cancellationToken, RequestRoute.Www);

            if (result.ValueKind != JsonValueKind.Array)
            {
                return (null, null);
            }

            Thing<Link>? post = null;
            Listing<Thing<Comment>>? comments = null;

            List<JsonElement> elements = result.EnumerateArray().ToList();
            if (elements.Count >= 1)
            {
                Listing<Thing<Link>>? postListing = JsonSerializer.Deserialize<Listing<Thing<Link>>>(elements[0].GetRawText(), _jsonOptions);
                post = postListing?.Data?.Children?.FirstOrDefault();
            }

            if (elements.Count >= 2)
            {
                comments = JsonSerializer.Deserialize<Listing<Thing<Comment>>>(elements[1].GetRawText(), _jsonOptions);
            }

            return (post, comments);
        }

        /// <inheritdoc />
        public async Task<Thing<Link>?> GetPostAsync(string idOrFullname, CancellationToken cancellationToken = default)
        {
            string id = idOrFullname.StartsWith("t3_", StringComparison.Ordinal) ? idOrFullname[3..] : idOrFullname;

            try
            {
                (Thing<Link>? post, _) = await this.GetCommentsAsync(id, limit: 1, cancellationToken: cancellationToken);
                return post;
            }
            catch (RedditHttpException ex) when (IsPostUnavailable(ex))
            {
                return null;
            }
        }

        /// <summary>
        /// Picks the path and host a listing answers on. See the class comment.
        /// </summary>
        private static (string Path, RequestRoute Route) RouteListing(string endpoint)
        {
            string path = $"/{endpoint.Trim('/')}";

            if (!IsMulti(path) && (path == "/hot" || path.EndsWith("/hot", StringComparison.Ordinal)))
            {
                path = path[..^"/hot".Length];
                path = path.Length == 0 ? "/" : path;
            }

            return (path, IsAll(path) ? RequestRoute.Old : RequestRoute.Www);
        }

        /// <summary>
        /// /r/all, which only old.reddit.com still serves: www redirects it to the home feed. The
        /// app's own name for it is "All": subreddit names aren't case-sensitive, and neither is this.
        /// </summary>
        private static bool IsAll(string path)
        {
            return path.Equals("/r/all", StringComparison.OrdinalIgnoreCase) || path.StartsWith("/r/all/", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsMulti(string path)
        {
            return path.Contains("/m/", StringComparison.Ordinal);
        }

        /// <summary>
        /// Whether Reddit is saying this one post can't be returned (gone, banned or private
        /// subreddit), which it answers with 404, or with 403 and a JSON reason. A 403 with an HTML
        /// body is the network security block, which must surface as the failure it is.
        /// </summary>
        private static bool IsPostUnavailable(RedditHttpException ex)
        {
            return ex.StatusCode == HttpStatusCode.NotFound
                || (ex.StatusCode == HttpStatusCode.Forbidden && ex.ResponseBody?.TrimStart().StartsWith('{') == true);
        }
    }
}
