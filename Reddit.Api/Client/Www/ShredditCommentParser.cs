using HtmlAgilityPack;
using Reddit.Api.Models;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Reads the comment tree www.reddit.com renders as HTML (the /svc/shreddit/comments and
    /// /svc/shreddit/more-comments partials) into the same <see cref="Comment"/> shape Reddit's JSON
    /// uses: comments with their replies nested, "load more" placeholders, and "continue this
    /// thread" placeholders. Throws on markup it does not recognise rather than guessing.
    ///
    /// Where each field comes from (checked against the JSON for the same comments):
    /// - &lt;shreddit-comment&gt;: id, author, depth, created, score, permalink, parent, post, collapsed,
    ///   and the deleted markers.
    /// - Its &lt;shreddit-comment-action-row&gt;: the user's vote and whether the score is hidden.
    /// - Its &lt;shreddit-overflow-menu&gt;: the author's account id, the subreddit id, saved, and reply
    ///   notifications (only rendered on the user's own comments, the only place they apply).
    /// - Its &lt;shreddit-comment-badges&gt;: distinguished, locked, and OP.
    /// - Its &lt;author-flair-event-handler&gt;: flair text, emoji, background and text colour.
    /// - The element with id "{id}-comment-rtjson-content": the body.
    ///
    /// What the markup does not carry: the body's markdown (<see cref="Comment.Body"/> is the body's
    /// text instead) and why a comment that isn't deleted was collapsed.
    ///
    /// Each element of a comment belongs to the nearest &lt;shreddit-comment&gt; around it, since replies
    /// are rendered inside their parent.
    /// </summary>
    public static partial class ShredditCommentParser
    {
        private const string CommentTag = "shreddit-comment";

        /// <summary>
        /// The comments in <paramref name="html"/>, as a tree. Comments whose parent isn't in the
        /// markup (all of them at the top of a thread, the first of a "load more" response) are the
        /// roots, with <see cref="Comment.ParentId"/> saying where they belong. A "load more" that
        /// isn't inside a comment (the thread's next top-level page) is a root too.
        /// </summary>
        /// <param name="linkFullname">The post the comments are on (t3_...).</param>
        /// <param name="subreddit">The subreddit the post is in, for comments rendered without a permalink.</param>
        public static List<Thing<Comment>> Parse(string html, string linkFullname, string subreddit)
        {
            HtmlDocument document = new();
            document.LoadHtml(html);

            List<Thing<Comment>> roots = [];
            Dictionary<string, Comment> byName = [];
            Dictionary<HtmlNode, Comment> byNode = [];

            foreach (HtmlNode node in document.DocumentNode.Descendants())
            {
                if (node.Name == CommentTag)
                {
                    Comment comment = ParseComment(node, linkFullname, subreddit);
                    Thing<Comment> thing = new() { Kind = ThingKind.Comment, Data = comment };

                    if (!byName.TryAdd(comment.Name, comment))
                    {
                        throw Unrecognised(comment.Name, "the comment is rendered twice");
                    }

                    byNode[node] = comment;

                    if (byName.TryGetValue(comment.ParentId, out Comment? parent))
                    {
                        Replies(parent).Add(thing);
                    }
                    else
                    {
                        roots.Add(thing);
                    }
                }
                else if (node.Name == "faceplate-partial" && node.GetAttributeValue("src", string.Empty).Contains("/more-comments/", StringComparison.Ordinal))
                {
                    HtmlNode? owner = Owner(node);
                    Comment? parent = owner == null ? null : byNode[owner];
                    Thing<Comment> more = new() { Kind = ThingKind.More, Data = ParseMore(node, parent, linkFullname) };

                    if (parent == null)
                    {
                        roots.Add(more);
                    }
                    else
                    {
                        Replies(parent).Add(more);
                    }
                }
            }

            // "Continue this thread": a comment with replies www didn't render and can't load inline.
            // Every comment with replies has the link; these have neither the replies nor a "load more".
            foreach ((HtmlNode node, Comment comment) in byNode)
            {
                bool hasLink = Owned(node, "a").Any(a => Classes(a).Contains("more-comments-link"));

                if (hasLink && comment.Replies == null)
                {
                    Replies(comment).Add(new Thing<Comment>
                    {
                        Kind = ThingKind.More,
                        Data = new Comment
                        {
                            Id = "_",
                            Name = "t1__",
                            ParentId = comment.Name,
                            LinkId = linkFullname,
                            Depth = comment.Depth + 1,
                            Children = [],
                            Count = 0
                        }
                    });
                }
            }

            return roots;
        }

        public static Comment ParseComment(HtmlNode node, string linkFullname, string subreddit)
        {
            string name = Required(node, "thingid", null);
            string id = name[(name.IndexOf('_') + 1)..];

            HtmlNode row = Owned(node, "shreddit-comment-action-row").FirstOrDefault()
                ?? throw Unrecognised(name, "no action row");
            HtmlNode badges = Owned(node, "shreddit-comment-badges").FirstOrDefault()
                ?? throw Unrecognised(name, "no badges element");

            bool commentDeleted = node.Attributes.Contains("is-comment-deleted");
            bool authorDeleted = node.Attributes.Contains("is-author-deleted");

            // A comment deleted by its author is rendered without its menu.
            HtmlNode? menu = Owned(node, "shreddit-overflow-menu").FirstOrDefault();

            if (menu == null && !commentDeleted)
            {
                throw Unrecognised(name, "no overflow menu");
            }

            HtmlNode? body = Owned(node, "div").FirstOrDefault(d => d.Id == $"{name}-comment-rtjson-content");

            string bodyText;
            string bodyHtml;
            CollapsedReasonCode collapsedReason = CollapsedReasonCode.Null;

            if (body != null)
            {
                bodyText = Text(body);
                bodyHtml = body.InnerHtml;
            }
            else if (authorDeleted)
            {
                // Reddit's JSON says [deleted] for a comment its author deleted, [removed] for one a moderator removed.
                bodyText = commentDeleted ? "[deleted]" : "[removed]";
                bodyHtml = $"<div class=\"md\"><p>{bodyText}</p>\n</div>";
                collapsedReason = CollapsedReasonCode.Deleted;
            }
            else
            {
                throw Unrecognised(name, "no body, and not deleted");
            }

            string permalink = node.Attr("permalink")
                ?? (commentDeleted ? $"/r/{subreddit}/comments/{linkFullname[3..]}/comment/{id}/" : throw Unrecognised(name, "no permalink"));

            (string? flairText, List<FlairRichtext>? flairRichtext, JsonColor flairBackground, FlairTextColor flairTextColor) = ParseFlair(node, name);

            return new Comment
            {
                Id = id,
                Name = name,
                ParentId = node.Attr("parentid") ?? linkFullname,
                LinkId = node.Attr("postid") ?? linkFullname,
                Depth = int.Parse(Required(node, "depth", name), CultureInfo.InvariantCulture),
                Permalink = permalink,
                Subreddit = subreddit,
                SubredditId = menu?.Attr("subreddit-id"),
                SubredditNamePrefixed = $"r/{subreddit}",

                Author = HtmlEntity.DeEntitize(Required(node, "author", name)),
                AuthorFullname = menu?.Attr("author-id"),
                AuthorFlairText = flairText,
                AuthorFlairRichtext = flairRichtext,
                AuthorFlairBackgroundColor = flairBackground,
                AuthorFlairTextColor = flairTextColor,

                Body = bodyText,
                BodyHtml = bodyHtml,
                CreatedUtc = ParseTime(name, Required(node, "created", name)),

                // Deleted comments are rendered without a score.
                Score = node.Attributes.Contains("score") ? int.Parse(node.Attr("score"), CultureInfo.InvariantCulture) : 0,
                ScoreHidden = row.Attributes.Contains("is-score-hidden"),
                Likes = row.Attr("vote-state") switch
                {
                    "UP" => VoteState.Upvote,
                    "DOWN" => VoteState.Downvote,
                    "NONE" => VoteState.None,
                    string other => throw Unrecognised(name, $"vote state '{other}'"),
                    null => throw Unrecognised(name, "no vote state")
                },

                Saved = menu?.Attributes.Contains("is-saved") == true,
                SendReplies = menu?.Attributes.Contains("is-send-replies") == true,
                Distinguished = badges.Attr("distinguished-as") switch
                {
                    null => DistinguishedKind.Null,
                    "MODERATOR" => DistinguishedKind.Moderator,
                    "ADMIN" => DistinguishedKind.Admin,
                    string other => throw Unrecognised(name, $"distinguished as '{other}'")
                },
                Locked = badges.Attributes.Contains("locked"),
                IsSubmitter = badges.Attributes.Contains("op"),
                Collapsed = node.Attributes.Contains("collapsed"),
                CollapsedReasonCode = collapsedReason
            };
        }

        private static Comment ParseMore(HtmlNode partial, Comment? parent, string linkFullname)
        {
            string url = HtmlEntity.DeEntitize(partial.GetAttributeValue("src", string.Empty));

            string cursor = partial.Descendants("input").FirstOrDefault(i => i.Attr("name") == "cursor")?.Attr("value")
                ?? throw Unrecognised(parent?.Name ?? linkFullname, $"load-more partial {url} has no cursor");

            // The count of replies it loads; the thread's next top-level page doesn't say.
            HtmlNode? number = partial.Descendants("faceplate-number").FirstOrDefault();

            string id = parent == null ? "more" : $"more_{parent.Id}";

            return new Comment
            {
                Id = id,
                Name = $"t1_{id}",
                ParentId = parent?.Name ?? linkFullname,
                LinkId = linkFullname,
                Depth = parent == null ? 0 : parent.Depth + 1,
                Children = [],
                Count = number == null ? 0 : int.Parse(number.Attr("number") ?? throw Unrecognised(id, "a reply count without a number"), CultureInfo.InvariantCulture),
                MoreUrl = url,
                MoreCursor = HtmlEntity.DeEntitize(cursor)
            };
        }

        /// <summary>
        /// The author's flair: a span whose inline style is the background, whose text class says
        /// light or dark text, holding emoji images (alt "emoji:name:") and text.
        /// </summary>
        private static (string? Text, List<FlairRichtext>? Richtext, JsonColor Background, FlairTextColor TextColor) ParseFlair(HtmlNode node, string name)
        {
            HtmlNode? handler = Owned(node, "author-flair-event-handler").FirstOrDefault();
            HtmlNode? span = handler?.Element("span");

            if (span == null)
            {
                return (null, null, JsonColor.Null, FlairTextColor.Null);
            }

            List<FlairRichtext> parts = [];

            foreach (HtmlNode part in span.Descendants())
            {
                if (part.Name == "img")
                {
                    string alt = part.GetAttributeValue("alt", string.Empty);

                    if (!alt.StartsWith("emoji:", StringComparison.Ordinal))
                    {
                        throw Unrecognised(name, $"flair image alt '{alt}'");
                    }

                    parts.Add(new FlairRichtext { Type = "emoji", EmojiId = alt["emoji".Length..], Url = HtmlEntity.DeEntitize(part.GetAttributeValue("src", string.Empty)) });
                }
                else if (part.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(part.InnerText))
                {
                    parts.Add(new FlairRichtext { Type = "text", Text = HtmlEntity.DeEntitize(part.InnerText.Trim()) });
                }
            }

            if (parts.Count == 0)
            {
                return (null, null, JsonColor.Null, FlairTextColor.Null);
            }

            // The parts in order, emoji as their codes. Reddit's own text spaces them as the user typed
            // it, which the markup loses; the app only shows this text with the codes stripped.
            string text = string.Concat(parts.Select(p => p.Type == "emoji" ? p.EmojiId : p.Text));

            Match background = BackgroundColor().Match(span.GetAttributeValue("style", string.Empty));
            HashSet<string> classes = Classes(span);

            FlairTextColor textColor = classes.Contains("text-global-white") ? FlairTextColor.Light
                : classes.Contains("text-secondary") ? FlairTextColor.Dark
                : throw Unrecognised(name, $"flair text colour classes '{span.GetAttributeValue("class", string.Empty)}'");

            JsonColor color = !background.Success ? JsonColor.Empty
                : background.Groups["color"].Value == "transparent" ? JsonColor.FromString("transparent")
                : JsonColor.FromString(background.Groups["color"].Value.ToLowerInvariant());

            return (text, parts, color, textColor);
        }

        /// <summary>The elements named <paramref name="name"/> inside <paramref name="comment"/> but not inside a reply.</summary>
        private static IEnumerable<HtmlNode> Owned(HtmlNode comment, string name)
        {
            return comment.Descendants(name).Where(n => Owner(n) == comment);
        }

        private static HtmlNode? Owner(HtmlNode node)
        {
            for (HtmlNode? parent = node.ParentNode; parent != null; parent = parent.ParentNode)
            {
                if (parent.Name == CommentTag)
                {
                    return parent;
                }
            }

            return null;
        }

        private static List<Thing<Comment>> Replies(Comment parent)
        {
            if (parent.Replies is not Listing<Thing<Comment>> listing)
            {
                listing = new Listing<Thing<Comment>> { Data = new ListingData<Thing<Comment>>() };
                parent.Replies = listing;
            }

            return listing.Data!.Children;
        }

        /// <summary>The body's text, a line per paragraph, with HTML entities decoded.</summary>
        private static string Text(HtmlNode body)
        {
            IEnumerable<string> lines = HtmlEntity.DeEntitize(body.InnerText)
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => l.Length > 0);

            return string.Join("\n", lines);
        }

        private static DateTime ParseTime(string name, string value)
        {
            // "2026-11-06T17:12:34.212000+0000": an offset without a colon, which .NET won't parse.
            Match time = CreatedTime().Match(value);

            return time.Success && DateTimeOffset.TryParse($"{time.Groups["time"].Value}{time.Groups["sign"].Value}{time.Groups["hours"].Value}:{time.Groups["minutes"].Value}", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset parsed)
                ? parsed.UtcDateTime
                : throw Unrecognised(name, $"created time '{value}'");
        }

        private static string Required(HtmlNode node, string attribute, string? name)
        {
            return node.GetAttributeValue(attribute, null)
                ?? throw Unrecognised(name ?? "a comment", $"no {attribute} attribute");
        }

        private static HashSet<string> Classes(HtmlNode node)
        {
            return [.. node.GetAttributeValue("class", string.Empty).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)];
        }

        private static InvalidOperationException Unrecognised(string name, string problem)
        {
            return new InvalidOperationException($"Unrecognised comment markup from www for {name}: {problem}.");
        }

        [GeneratedRegex(@"background-color:\s*(?<color>#[0-9A-Fa-f]{3,8}|transparent)")]
        private static partial Regex BackgroundColor();

        [GeneratedRegex(@"^(?<time>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?)(?<sign>[+-])(?<hours>\d{2})(?<minutes>\d{2})$")]
        private static partial Regex CreatedTime();
    }
}
