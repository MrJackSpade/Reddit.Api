using HtmlAgilityPack;
using System.Text;
using System.Web;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Reads which posts a www.reddit.com listing partial holds, in order, and where its next page
    /// starts. The posts themselves are fetched separately (see <see cref="RedditClient.GetByIdAsync"/>).
    ///
    /// - Feeds (/svc/shreddit/community-more-posts/..., /svc/shreddit/feeds/...): each post is a
    ///   &lt;shreddit-post id="t3_..."&gt;; ads are &lt;shreddit-ad-post&gt;, so they aren't read. The next
    ///   page is the &lt;faceplate-partial slot="load-after"&gt;, whose after parameter is the last post's
    ///   fullname in base64.
    /// - Search (/svc/shreddit/search/...): each result is an element with
    ///   data-testid="sdui-post-unit" (ads aren't), in every sort's layout, holding a title link
    ///   whose id is "search-post-title-t3_...". The next page is the partial whose src carries a
    ///   cursor, an opaque token.
    /// </summary>
    public static class ShredditListingParser
    {
        /// <summary>The posts in a feed partial, and the fullname the next page starts after (null at the end).</summary>
        public static (List<string> Names, string? After) ParseFeed(string html)
        {
            HtmlDocument document = Load(html);

            List<string> names = document.DocumentNode.Descendants("shreddit-post")
                .Select(post => post.Attr("id") ?? throw new InvalidOperationException("A post in www's feed has no id."))
                .Distinct()
                .ToList();

            HtmlNode? next = document.DocumentNode.Descendants("faceplate-partial")
                .SingleOrDefault(partial => partial.Attr("slot") == "load-after");

            string? after = null;

            if (next != null)
            {
                string encoded = Parameter(next, "after")
                    ?? throw new InvalidOperationException($"www's next-page partial has no after: {next.GetAttributeValue("src", string.Empty)}");

                after = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            }

            return (names, after);
        }

        /// <summary>The posts in a search partial, and the cursor of the next page (null at the end).</summary>
        public static (List<string> Names, string? Cursor) ParseSearch(string html)
        {
            HtmlDocument document = Load(html);

            List<string> names = document.DocumentNode.Descendants()
                .Where(node => node.Attr("data-testid") == "sdui-post-unit")
                .Select(PostUnitName)
                .Distinct()
                .ToList();

            string? cursor = document.DocumentNode.Descendants("faceplate-partial")
                .Where(partial => partial.GetAttributeValue("src", string.Empty).StartsWith("/svc/shreddit/", StringComparison.Ordinal))
                .Select(partial => Parameter(partial, "cursor"))
                .SingleOrDefault(value => value != null);

            return (names, cursor);
        }

        /// <summary>A search result's fullname, from its title link's id ("search-post-title-t3_...").</summary>
        private static string PostUnitName(HtmlNode unit)
        {
            const string prefix = "search-post-title-";

            string id = unit.Descendants().FirstOrDefault(node => node.Attr("data-testid") == "post-title-text")?.Attr("id")
                ?? throw new InvalidOperationException("A result in www's search has no title link with an id.");

            return id.StartsWith(prefix + "t3_", StringComparison.Ordinal)
                ? id[prefix.Length..]
                : throw new InvalidOperationException($"A result in www's search has a title link with the id '{id}'.");
        }

        /// <summary>The base64 www puts in a feed's after parameter.</summary>
        public static string EncodeAfter(string fullname)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(fullname));
        }

        private static HtmlDocument Load(string html)
        {
            HtmlDocument document = new();
            document.LoadHtml(html);
            return document;
        }

        private static string? Parameter(HtmlNode partial, string name)
        {
            string src = HtmlEntity.DeEntitize(partial.GetAttributeValue("src", string.Empty));
            int query = src.IndexOf('?');

            return query < 0 ? null : HttpUtility.ParseQueryString(src[(query + 1)..])[name];
        }
    }
}
