using HtmlAgilityPack;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Reads the post from a www.reddit.com thread page (/r/{sub}/comments/{id}/...), as a browser has
    /// it once the page's JavaScript challenge has passed: the &lt;shreddit-post&gt; the page is for.
    /// See <see cref="ShredditPostParser"/>.
    ///
    /// Only the post: the page's comments lose the user's vote, saved and reply state once the page's
    /// scripts have run (they move it out of the HTML a browser hands back), so comments come from
    /// www's comments partial instead (<see cref="RedditClient.GetThreadCommentsAsync"/>).
    /// </summary>
    public static class ShredditThreadParser
    {
        public static Link ParsePost(string html)
        {
            HtmlDocument document = new();
            document.LoadHtml(html);

            HtmlNode post = document.DocumentNode.Descendants("shreddit-post").FirstOrDefault(p => p.Attr("view-context") == "CommentsPage")
                ?? throw new InvalidOperationException("The page has no thread post (<shreddit-post view-context=\"CommentsPage\">): it isn't a www thread page.");

            return ShredditPostParser.Parse(post);
        }
    }
}
