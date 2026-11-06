using HtmlAgilityPack;
using Reddit.Api.Models;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Common;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Reads the things that old.reddit.com's /api/morechildren renders as HTML (requested without
    /// the .json suffix, which Reddit blocks) into the same <see cref="Comment"/> shape Reddit's JSON
    /// uses, for both comments and "more" placeholders. Throws on markup it does not recognise
    /// rather than guessing.
    ///
    /// The markup does not carry: flair emoji, flair colours, flair template IDs, award counts,
    /// reply state, or moderator fields. Those stay at their defaults.
    /// </summary>
    public static partial class RenderedThingParser
    {
        public static Thing<Comment> Parse(MoreChildrenThing thing, string linkFullname)
        {
            MoreChildrenThingData data = thing.Data
                ?? throw new InvalidOperationException($"Reddit's /api/morechildren response for {linkFullname} has a {thing.Kind} without data.");

            return thing.Kind switch
            {
                ThingKind.Comment => new Thing<Comment> { Kind = ThingKind.Comment, Data = ParseComment(data) },
                ThingKind.More => new Thing<Comment> { Kind = ThingKind.More, Data = ParseMore(data) },
                _ => throw new InvalidOperationException($"Reddit's /api/morechildren response for {linkFullname} has an unexpected {thing.Kind} ({data.Id}).")
            };
        }

        public static Comment ParseComment(MoreChildrenThingData data)
        {
            HtmlNode thing = LoadThing(data);

            HtmlNode entry = thing.SelectSingleNode("./div[contains(concat(' ', normalize-space(@class), ' '), ' entry ')]")
                ?? throw Unrecognised(data, "no entry element");

            HtmlNode tagline = entry.SelectSingleNode(".//p[contains(concat(' ', normalize-space(@class), ' '), ' tagline ')]")
                ?? throw Unrecognised(data, "no tagline element");

            HtmlNode created = tagline.SelectSingleNode("./time[contains(concat(' ', normalize-space(@class), ' '), ' live-timestamp ')]")
                ?? throw Unrecognised(data, "no created time");

            HashSet<string> thingClasses = Classes(thing);
            HashSet<string> entryClasses = Classes(entry);

            HtmlNode? author = tagline.SelectSingleNode("./a[contains(concat(' ', normalize-space(@class), ' '), ' author ')]");
            HashSet<string> authorClasses = author == null ? [] : Classes(author);

            bool scoreHidden = tagline.SelectSingleNode("./span[contains(concat(' ', normalize-space(@class), ' '), ' score-hidden ')]") != null;
            VoteState likes = entryClasses.Contains("likes") ? VoteState.Upvote : entryClasses.Contains("dislikes") ? VoteState.Downvote : VoteState.None;

            // Deleted comments are rendered without a score.
            int score = scoreHidden || thingClasses.Contains("deleted") ? 0 : ParseScore(data, tagline, likes);

            HtmlNode? flair = tagline.SelectSingleNode("./span[contains(concat(' ', normalize-space(@class), ' '), ' flair ')]");
            HtmlNode? edited = tagline.SelectSingleNode("./time[contains(concat(' ', normalize-space(@class), ' '), ' edited-timestamp ')]");

            return new Comment
            {
                Id = ShortId(data.Id),
                Name = data.Id,
                ParentId = data.Parent,
                LinkId = data.Link,
                Body = data.ContentText,
                BodyHtml = data.ContentHtml,

                // Deleted comments have no author element or data-author; Reddit's JSON says [deleted].
                Author = HtmlEntity.DeEntitize(thing.GetAttributeValue("data-author", "[deleted]")),
                AuthorFullname = NullIfEmpty(thing.GetAttributeValue("data-author-fullname", string.Empty)),
                IsSubmitter = authorClasses.Contains("submitter"),
                Distinguished = authorClasses.Contains("moderator") ? DistinguishedKind.Moderator
                    : authorClasses.Contains("admin") ? DistinguishedKind.Admin
                    : DistinguishedKind.Null,

                AuthorFlairText = flair == null ? null : HtmlEntity.DeEntitize(flair.GetAttributeValue("title", flair.InnerText)),
                AuthorFlairCssClass = flair == null ? null : Classes(flair).FirstOrDefault(c => c.StartsWith("flair-", StringComparison.Ordinal))?["flair-".Length..],

                Subreddit = thing.GetAttributeValue("data-subreddit", string.Empty),
                SubredditId = NullIfEmpty(thing.GetAttributeValue("data-subreddit-fullname", string.Empty)),
                SubredditNamePrefixed = thing.GetAttributeValue("data-subreddit-prefixed", string.Empty),
                SubredditType = JsonSerializer.Deserialize<SubredditType>(JsonSerializer.Serialize(thing.GetAttributeValue("data-subreddit-type", string.Empty))),
                Permalink = HtmlEntity.DeEntitize(thing.GetAttributeValue("data-permalink", string.Empty)),

                Score = score,
                Ups = score,
                ScoreHidden = scoreHidden,
                Likes = likes,
                CreatedUtc = ParseTime(data, created),
                Edited = edited == null ? JsonDateTime.Empty : ParseTime(data, edited),
                Gilded = thing.GetAttributeValue("data-gildings", 0),

                Stickied = thingClasses.Contains("stickied"),
                Saved = thingClasses.Contains("saved"),
                Locked = thingClasses.Contains("locked"),
                Archived = thingClasses.Contains("archived"),
                Controversiality = thingClasses.Contains("controversial") ? 1 : 0,
                Collapsed = thingClasses.Contains("collapsed") || thingClasses.Contains("collapsed-for-reason") || thingClasses.Contains("deleted"),
                CollapsedReasonCode = thingClasses.Contains("deleted") ? CollapsedReasonCode.Deleted
                    : thingClasses.Contains("collapsed-for-reason") ? CollapsedReasonCode.LowScore
                    : CollapsedReasonCode.Null
            };
        }

        public static Comment ParseMore(MoreChildrenThingData data)
        {
            HtmlNode thing = LoadThing(data);

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

                count = int.Parse(replies.Groups["count"].Value, CultureInfo.InvariantCulture);
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
                Id = ShortId(data.Id),
                Name = data.Id,
                ParentId = data.Parent,
                LinkId = data.Link,
                Children = children,
                Count = count
            };
        }

        private static HtmlNode LoadThing(MoreChildrenThingData data)
        {
            HtmlDocument document = new();
            document.LoadHtml(data.Content);

            HtmlNode thing = document.DocumentNode.SelectSingleNode("//div[contains(concat(' ', normalize-space(@class), ' '), ' thing ')]")
                ?? throw Unrecognised(data, "no thing element");

            // Deleted comments are rendered without data-fullname.
            string? fullname = thing.GetAttributeValue("data-fullname", null);

            if (fullname != null && fullname != data.Id)
            {
                throw Unrecognised(data, $"data-fullname '{fullname}' does not match the thing's id");
            }

            return thing;
        }

        /// <summary>
        /// The tagline renders the score three times, for each vote state the page can flip between;
        /// the entry's class says which is current.
        /// </summary>
        private static int ParseScore(MoreChildrenThingData data, HtmlNode tagline, VoteState likes)
        {
            string state = likes switch { VoteState.Upvote => "likes", VoteState.Downvote => "dislikes", _ => "unvoted" };

            HtmlNode score = tagline.SelectSingleNode($"./span[contains(concat(' ', normalize-space(@class), ' '), ' score ') and contains(concat(' ', normalize-space(@class), ' '), ' {state} ')]")
                ?? throw Unrecognised(data, $"no {state} score");

            string title = score.GetAttributeValue("title", string.Empty);

            return int.TryParse(title, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int value)
                ? value
                : throw Unrecognised(data, $"score title '{title}' is not a number");
        }

        private static DateTime ParseTime(MoreChildrenThingData data, HtmlNode time)
        {
            string value = time.GetAttributeValue("datetime", string.Empty);

            return DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset parsed)
                ? parsed.UtcDateTime
                : throw Unrecognised(data, $"datetime '{value}' is not a time");
        }

        private static HashSet<string> Classes(HtmlNode node)
        {
            return [.. node.GetAttributeValue("class", string.Empty).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)];
        }

        private static string ShortId(string fullname)
        {
            return fullname[(fullname.IndexOf('_') + 1)..];
        }

        private static string? NullIfEmpty(string value)
        {
            return value.Length == 0 ? null : value;
        }

        private static InvalidOperationException Unrecognised(MoreChildrenThingData data, string problem)
        {
            return new InvalidOperationException($"Unrecognised markup from /api/morechildren for {data.Id}: {problem}. Markup: {data.Content}");
        }

        [GeneratedRegex(@"morechildren\(\s*this\s*,\s*'[^']*'\s*,\s*'[^']*'\s*,\s*'(?<children>[^']*)'")]
        private static partial Regex MoreChildrenCall();

        [GeneratedRegex(@"\((?<count>\d+)\s+repl(?:y|ies)\)")]
        private static partial Regex ReplyCount();
    }
}
