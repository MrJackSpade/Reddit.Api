using HtmlAgilityPack;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.LinksComments;
using System.Text.RegularExpressions;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Reads a "more" placeholder from the HTML that /api/morechildren renders when requested
    /// without the .json suffix, into the same <see cref="Comment"/> shape Reddit's JSON uses for
    /// kind "more". Throws on markup it does not recognise rather than guessing.
    /// </summary>
    public static partial class RenderedMoreParser
    {
        public static Comment Parse(MoreChildrenThingData data)
        {
            HtmlDocument document = new();
            document.LoadHtml(data.Content);

            HtmlNode thing = document.DocumentNode.SelectSingleNode("//div[@data-fullname]")
                ?? throw Unrecognised(data, "no element with data-fullname");

            string fullname = thing.GetAttributeValue("data-fullname", string.Empty);

            if (fullname != data.Id)
            {
                throw Unrecognised(data, $"data-fullname '{fullname}' does not match the thing's id");
            }

            HtmlNode? loadMore = thing.SelectSingleNode(".//span[contains(concat(' ', normalize-space(@class), ' '), ' morecomments ')]/a[@onclick]");
            HtmlNode? deepThread = thing.SelectSingleNode(".//span[contains(concat(' ', normalize-space(@class), ' '), ' deepthread ')]/a");

            List<string> children;
            int count;

            if (loadMore != null)
            {
                // onclick="return morechildren(this, 't3_link', 'sort', 'id1,id2', 'False')"
                Match call = MoreChildrenCall().Match(HtmlEntity.DeEntitize(loadMore.GetAttributeValue("onclick", string.Empty)));

                if (!call.Success)
                {
                    throw Unrecognised(data, "load-more link has no morechildren(...) call");
                }

                children = [.. call.Groups["children"].Value.Split(',', StringSplitOptions.RemoveEmptyEntries)];

                // "load more comments (4 replies)"
                Match replies = ReplyCount().Match(HtmlEntity.DeEntitize(loadMore.InnerText));

                if (!replies.Success)
                {
                    throw Unrecognised(data, "load-more link has no reply count");
                }

                count = int.Parse(replies.Groups["count"].Value);
            }
            else if (deepThread != null)
            {
                // "continue this thread": Reddit's JSON gives these no children.
                children = [];
                count = 0;
            }
            else
            {
                throw Unrecognised(data, "neither a load-more nor a continue-this-thread link");
            }

            return new Comment
            {
                Id = fullname[(fullname.IndexOf('_') + 1)..],
                Name = fullname,
                ParentId = data.Parent,
                Children = children,
                Count = count
            };
        }

        private static InvalidOperationException Unrecognised(MoreChildrenThingData data, string problem)
        {
            return new InvalidOperationException($"Unrecognised \"more\" markup from /api/morechildren for {data.Id}: {problem}. Markup: {data.Content}");
        }

        [GeneratedRegex(@"morechildren\(\s*this\s*,\s*'[^']*'\s*,\s*'[^']*'\s*,\s*'(?<children>[^']*)'")]
        private static partial Regex MoreChildrenCall();

        [GeneratedRegex(@"\((?<count>\d+)\s+repl(?:y|ies)\)")]
        private static partial Regex ReplyCount();
    }
}
