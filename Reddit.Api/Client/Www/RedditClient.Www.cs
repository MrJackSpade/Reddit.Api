using Reddit.Api.Exceptions;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using System.Net;
using System.Text.Json;

namespace Reddit.Api.Client
{
    /// <summary>
    /// The calls sent to www.reddit.com with the browser session cookies.
    ///
    /// CONTEXT (the wider story is on the OAuth partial): www is where calls should end up, for
    /// parity with what Reddit's main site serves. Its comment thread JSON still works, and threads
    /// are refused on OAuth, so everything that needs a post's full data goes through it. Listing
    /// JSON (/r/all.json), /search.json and /api/ paths are blocked here for the session.
    ///
    /// /api/info and /by_id were how posts were fetched by ID. Both were blocked on 2026-11-06, so a
    /// post is now fetched through its thread, which means one request per post: History pages and
    /// search results cost one request per result rather than one per page.
    ///
    /// Comments can't be fetched by ID alone any more either: every thread URL needs the post ID
    /// (/comments/{post}/_/{comment}.json), and nothing else returns a lone comment.
    ///
    /// Next to move here: "load more" and search, which are on old.reddit.com. See that partial.
    /// </summary>
    public partial class RedditClient
    {
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
