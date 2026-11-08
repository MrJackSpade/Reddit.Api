using HtmlAgilityPack;
using Reddit.Api.Exceptions;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Search;
using System.Net;
using System.Text.RegularExpressions;

namespace Reddit.Api.Client
{
    /// <summary>
    /// The calls sent to old.reddit.com with the browser session cookies, as rendered HTML.
    ///
    /// CONTEXT (the wider story is on the OAuth partial): this is the holding area for calls with no
    /// working JSON form anywhere. It should shrink, not grow. old.reddit.com is the old site, and it
    /// lacks some state: a comment's reply state isn't tracked here even in a browser.
    ///
    /// What is here and why:
    /// - "Load more" (not used by the app since 2026-11-08: threads, and their "load more", come
    ///   from www's HTML; see the www partial): /api/morechildren.json is blocked everywhere, and so is /api/info, which
    ///   supplied the comment data for a while. The suffix-less endpoint still answers, rendering
    ///   each comment as HTML, so <see cref="RenderedThingParser"/> reads comments from that markup.
    ///   It's a form POST because a big "more" lists over a thousand children, which as a query
    ///   string fails with 414. Comments loaded this way have no flair emoji, flair colours or award
    ///   counts, and no reply state, because the markup doesn't carry them.
    /// - /r/all listings: www no longer has /r/all at all (it redirects to the home feed), and this
    ///   host still answers it. The www partial's listing router sends them here, and its "new" and
    ///   "rising" sorts come from this HTML page (IDs and the next page's anchor; the posts themselves
    ///   from www, one request each). Beware ?sort=new or ?sort=rising: they answer 200 with the
    ///   hot listing.
    ///
    /// Moved to www (2026-11-08): search and the other "new" and "rising" listings, from www's
    /// /svc/shreddit partials. See RedditClient.Www.cs.
    ///
    /// How "load more" moved to www: its load-more (POST /svc/shreddit/more-comments/...) locates
    /// comments by a tree position and a cursor that only exist in www's own thread HTML, so thread
    /// loading moved to that HTML with it (GetThreadCommentsAsync, GetMoreCommentsAsync).
    ///
    /// These calls need the browser session, because OAuth doesn't serve the rendered form. Without
    /// one they throw rather than quietly asking a different host.
    /// </summary>
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<List<Thing<Comment>>?> GetMoreChildrenAsync(
            string linkFullname,
            IEnumerable<string> children,
            string? sort = null,
            int? depth = null,
            string? subreddit = null,
            CancellationToken cancellationToken = default)
        {
            await this.EnsureWebSessionAsync("Loading more comments", cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["link_id"] = linkFullname,
                ["children"] = string.Join(",", children),
                ["limit_children"] = "false"
            };

            if (!string.IsNullOrEmpty(sort))
            {
                formData["sort"] = sort;
            }

            if (depth != null)
            {
                formData["depth"] = depth.Value.ToString();
            }

            // Without the subreddit, Reddit leaves user flair out of the rendered comments.
            if (!string.IsNullOrEmpty(subreddit))
            {
                formData["r"] = subreddit;
            }

            // raw_json=1 keeps the rendered markup unescaped.
            ApiResponse<MoreChildrenResponseData>? response = await this.PostFormAsync<ApiResponse<MoreChildrenResponseData>>("/api/morechildren?raw_json=1", formData, cancellationToken, RequestRoute.Old, jsonSuffix: false);
            CheckApiResponse(response);

            if (response?.Json?.Data?.Things == null)
            {
                throw new InvalidOperationException($"Reddit's /api/morechildren response for {linkFullname} has no things list.");
            }

            return response.Json.Data.Things.Select(thing => RenderedThingParser.Parse(thing, linkFullname)).ToList();
        }

        private async Task<Listing<Thing<Link>>?> GetPostsFromListingHtmlAsync(string path, ListingParameters? parameters, CancellationToken cancellationToken)
        {
            await this.EnsureWebSessionAsync("This sort", cancellationToken);

            // The trailing slash avoids a redirect to the canonical URL.
            string endpoint = $"/{path.Trim('/')}/{parameters?.ToQueryString() ?? string.Empty}";

            return await this.GetPostsFromHtmlAsync(endpoint, "//div[@id='siteTable']", " link ", cancellationToken);
        }

        /// <summary>
        /// Reads the post IDs and next page anchor from a rendered listing page, and fetches the
        /// posts themselves from www.
        /// </summary>
        /// <param name="pageMarkerXPath">
        /// An element only the expected page has. A page with no results has no result elements, so
        /// this is what tells an empty result from a page that isn't the listing at all.
        /// </param>
        /// <param name="resultClass">The space-padded class that marks a result element.</param>
        private async Task<Listing<Thing<Link>>?> GetPostsFromHtmlAsync(string endpoint, string pageMarkerXPath, string resultClass, CancellationToken cancellationToken)
        {
            string html = await this.GetHtmlAsync(endpoint, RequestRoute.Old, cancellationToken);

            HtmlDocument document = new();
            document.LoadHtml(html);

            if (document.DocumentNode.SelectSingleNode(pageMarkerXPath) == null)
            {
                throw new InvalidOperationException($"Reddit's response to {endpoint} is not the expected listing page.");
            }

            List<string> names = (document.DocumentNode.SelectNodes($"//div[contains(concat(' ', normalize-space(@class), ' '), '{resultClass}')]") ?? Enumerable.Empty<HtmlNode>())
                .Where(node => !node.GetAttributeValue("class", string.Empty).Contains("promoted", StringComparison.Ordinal))
                .Select(node => node.GetAttributeValue("data-fullname", string.Empty))
                .Select(name => name.Length > 0 ? name : throw new InvalidOperationException($"A result in Reddit's response to {endpoint} has no data-fullname."))
                .ToList();

            Listing<Thing<Link>>? posts = await this.GetByIdAsync(names, cancellationToken);

            HtmlNode? next = document.DocumentNode.SelectSingleNode("//a[contains(@rel,'next')]");
            Match after = Regex.Match(WebUtility.HtmlDecode(next?.GetAttributeValue("href", string.Empty) ?? string.Empty), @"[?&]after=(t3_[a-z0-9]+)");

            posts!.Data!.After = after.Success ? after.Groups[1].Value : null;
            return posts;
        }

        private async Task EnsureWebSessionAsync(string action, CancellationToken cancellationToken)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            if (!HasWebSession)
            {
                throw new RedditAuthenticationException($"{action} needs a browser session login: Reddit only serves it to the website.");
            }
        }
    }
}
