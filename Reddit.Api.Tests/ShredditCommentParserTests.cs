using Reddit.Api.Client;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    /// <summary>
    /// Fixtures are www.reddit.com's /svc/shreddit/comments and /more-comments responses, cut down
    /// to a few comment trees with the scripts, styles, composers and menus removed. The expected
    /// values were checked against Reddit's JSON for the same comments.
    /// </summary>
    [TestClass]
    public class ShredditCommentParserTests
    {
        private static List<Thing<Comment>> Parse(string fixture, string linkFullname, string subreddit)
        {
            string html = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", fixture));
            return ShredditCommentParser.Parse(html, linkFullname, subreddit);
        }

        private static List<Thing<Comment>> Thread() => Parse("shreddit-comments-thread.html", "t3_1xpfn3x", "NintendoSwitch");

        private static List<Thing<Comment>> Replies(Comment comment) => ((Listing<Thing<Comment>>)comment.Replies!).Data!.Children;

        private static IEnumerable<Thing<Comment>> All(List<Thing<Comment>> things)
        {
            foreach (Thing<Comment> thing in things)
            {
                yield return thing;

                if (thing.Data!.Replies is Listing<Thing<Comment>>)
                {
                    foreach (Thing<Comment> reply in All(Replies(thing.Data)))
                    {
                        yield return reply;
                    }
                }
            }
        }

        private static Comment Find(List<Thing<Comment>> things, string name) => All(things).Single(t => t.Data!.Name == name).Data!;

        [TestMethod]
        public void Parse_ReadsTheTopLevelComment()
        {
            List<Thing<Comment>> roots = Thread();
            Comment comment = roots[0].Data!;

            Assert.AreEqual(ThingKind.Comment, roots[0].Kind);
            Assert.AreEqual("19s8j7lh7xeg1", comment.Id);
            Assert.AreEqual("t1_19s8j7lh7xeg1", comment.Name);
            Assert.AreEqual("t3_1xpfn3x", comment.ParentId);
            Assert.AreEqual("t3_1xpfn3x", comment.LinkId);
            Assert.AreEqual(0, comment.Depth);
            Assert.AreEqual("PinkestGuy", comment.Author);
            Assert.AreEqual("t2_13m1ho", comment.AuthorFullname);
            Assert.AreEqual("t5_3h47q", comment.SubredditId);
            Assert.AreEqual("NintendoSwitch", comment.Subreddit);
            Assert.AreEqual("/r/NintendoSwitch/comments/1xpfn3x/comment/19s8j7lh7xeg1/", comment.Permalink);
            Assert.AreEqual(567, comment.Score);
            Assert.AreEqual(VoteState.None, comment.Likes);
            Assert.AreEqual(DateTimeOffset.Parse("2026-11-06T17:12:34.212+00:00").UtcDateTime, comment.CreatedUtc.Value);
            Assert.IsTrue(comment.Body.StartsWith("It's really well made. But I really wonder why the", StringComparison.Ordinal));
            Assert.IsTrue(comment.BodyHtml!.Contains("<p>", StringComparison.Ordinal));
            Assert.AreEqual(7, Replies(comment).Count);
        }

        [TestMethod]
        public void Parse_NestsRepliesUnderTheirParent()
        {
            List<Thing<Comment>> roots = Thread();

            foreach (Thing<Comment> parent in All(roots).Where(t => t.Data!.Replies is Listing<Thing<Comment>>))
            {
                foreach (Thing<Comment> reply in Replies(parent.Data!))
                {
                    Assert.AreEqual(parent.Data!.Name, reply.Data!.ParentId);
                    Assert.AreEqual(parent.Data.Depth + 1, reply.Data.Depth);
                }
            }

            Assert.AreEqual(25, All(roots).Count(t => t.Kind == ThingKind.Comment));
        }

        [TestMethod]
        public void Parse_ReadsTheUsersVotes()
        {
            List<Thing<Comment>> roots = Thread();

            Assert.AreEqual(VoteState.Downvote, Find(roots, "t1_19s8kpo1r3vqb").Likes);
            Assert.AreEqual(VoteState.Upvote, Find(roots, "t1_19s9eu8wxfa79").Likes);
        }

        [TestMethod]
        public void Parse_ReadsBadges()
        {
            List<Thing<Comment>> roots = Thread();

            Comment mod = Find(roots, "t1_19sazbsx0vlgk");
            Assert.AreEqual(DistinguishedKind.Moderator, mod.Distinguished);
            Assert.IsTrue(mod.Locked);

            Assert.IsTrue(Find(roots, "t1_19s8p1170prac").IsSubmitter);
            Assert.AreEqual(DistinguishedKind.Null, Find(roots, "t1_19s8j7lh7xeg1").Distinguished);
        }

        [TestMethod]
        public void Parse_ReadsFlair()
        {
            List<Thing<Comment>> roots = Thread();

            Comment colored = Find(roots, "t1_19s8l5fjy4rj5");
            Assert.AreEqual("#e60213", colored.AuthorFlairBackgroundColor.ToNullableString());
            Assert.AreEqual(FlairTextColor.Light, colored.AuthorFlairTextColor);
            Assert.AreEqual(2, colored.AuthorFlairRichtext!.Count);
            Assert.AreEqual("emoji", colored.AuthorFlairRichtext[0].Type);
            Assert.AreEqual(":developer:", colored.AuthorFlairRichtext[0].EmojiId);
            Assert.IsTrue(colored.AuthorFlairRichtext[0].Url!.StartsWith("https://emoji.redditmedia.com/", StringComparison.Ordinal));
            Assert.AreEqual("text", colored.AuthorFlairRichtext[1].Type);
            Assert.AreEqual("BitFinity", colored.AuthorFlairRichtext[1].Text);

            Comment emojiOnly = Find(roots, "t1_19s8lwee66cf4");
            Assert.AreEqual("transparent", emojiOnly.AuthorFlairBackgroundColor.ToNullableString());
            Assert.AreEqual(FlairTextColor.Dark, emojiOnly.AuthorFlairTextColor);
            Assert.AreEqual(":link-botw:", emojiOnly.AuthorFlairRichtext!.Single().EmojiId);

            Comment none = Find(roots, "t1_19s8j7lh7xeg1");
            Assert.IsNull(none.AuthorFlairRichtext);
            Assert.IsNull(none.AuthorFlairText);
        }

        [TestMethod]
        public void Parse_ReadsDeletedAndRemovedComments()
        {
            List<Thing<Comment>> roots = Thread();

            Comment deleted = Find(roots, "t1_19s8mkbwp7c6c");
            Assert.AreEqual("[deleted]", deleted.Author);
            Assert.AreEqual("[deleted]", deleted.Body);
            Assert.AreEqual(CollapsedReasonCode.Deleted, deleted.CollapsedReasonCode);
            Assert.AreEqual("/r/NintendoSwitch/comments/1xpfn3x/comment/19s8mkbwp7c6c/", deleted.Permalink);

            Comment removed = Find(roots, "t1_19sat6a9xss4k");
            Assert.AreEqual("[deleted]", removed.Author);
            Assert.AreEqual("[removed]", removed.Body);
            Assert.AreEqual(CollapsedReasonCode.Deleted, removed.CollapsedReasonCode);
            Assert.AreEqual(1, Replies(removed).Count);
        }

        [TestMethod]
        public void Parse_MarksCollapsedComments()
        {
            Comment lowScore = Find(Thread(), "t1_19s8l0mv6l8wx");

            Assert.IsTrue(lowScore.Collapsed);
            Assert.AreEqual(CollapsedReasonCode.LowScore, lowScore.CollapsedReasonCode);
            Assert.AreEqual(-21, lowScore.Score);
            Assert.AreEqual("You don't play with your phone next to you?", lowScore.Body);
        }

        [TestMethod]
        public void Parse_ReadsLoadMorePlaceholders()
        {
            List<Thing<Comment>> roots = Thread();

            Comment more = Replies(Find(roots, "t1_19s8nldf30t42")).Single().Data!;
            Assert.AreEqual(ThingKind.More, Replies(Find(roots, "t1_19s8nldf30t42")).Single().Kind);
            Assert.AreEqual("t1_19s8nldf30t42", more.ParentId);
            Assert.AreEqual(8, more.Count);
            Assert.AreEqual(5, more.Depth);
            Assert.AreEqual("/svc/shreddit/more-comments/r/NintendoSwitch/t3_1xpfn3x?sort=TOP&startingDepth=5&commentParentPositions=0,0,1,0,0", more.MoreUrl);
            Assert.IsFalse(string.IsNullOrEmpty(more.MoreCursor));
            Assert.AreEqual(0, more.Children!.Count);

            // The thread's next top-level page.
            Thing<Comment> next = roots[1];
            Assert.AreEqual(ThingKind.More, next.Kind);
            Assert.AreEqual("t3_1xpfn3x", next.Data!.ParentId);
            Assert.IsTrue(next.Data.MoreUrl!.Contains("top-level=1", StringComparison.Ordinal));
            Assert.IsFalse(string.IsNullOrEmpty(next.Data.MoreCursor));
        }

        [TestMethod]
        public void Parse_ReadsContinueThisThread()
        {
            List<Thing<Comment>> roots = Parse("shreddit-comments-continue-thread.html", "t3_1xqm731", "youtube");

            Thing<Comment> placeholder = Replies(Find(roots, "t1_19sb4nzn4o95u")).Single();
            Assert.AreEqual(ThingKind.More, placeholder.Kind);
            Assert.AreEqual("t1_19sb4nzn4o95u", placeholder.Data!.ParentId);
            Assert.AreEqual(0, placeholder.Data.Children!.Count);
            Assert.IsNull(placeholder.Data.MoreUrl);

            // Comments with their replies, or a load-more, get no continue placeholder.
            Assert.AreEqual(1, All(roots).Count(t => t.Kind == ThingKind.More && t.Data!.MoreUrl == null));
        }

        [TestMethod]
        public void Parse_ReadsTheUsersOwnCommentState()
        {
            List<Thing<Comment>> roots = Parse("shreddit-comments-own.html", "t3_1xm4wpl", "retroid");

            Comment own = Find(roots, "t1_19s5paqcchvls");
            Assert.AreEqual("mrjackspade", own.Author);
            Assert.IsTrue(own.SendReplies);
            Assert.AreEqual(VoteState.Upvote, own.Likes);

            Assert.IsFalse(Find(roots, "t1_19s1nv4ou2lad").SendReplies);
        }

        [TestMethod]
        public void Parse_ReadsALoadMoreResponse()
        {
            List<Thing<Comment>> roots = Parse("shreddit-more-comments.html", "t3_1xpfn3x", "NintendoSwitch");

            // The comments it loads come first, attached to a parent outside the response.
            Assert.AreEqual(ThingKind.Comment, roots[0].Kind);
            Assert.IsTrue(roots[0].Data!.ParentId.StartsWith("t1_", StringComparison.Ordinal));
            Assert.AreEqual(3, All(roots).Count(t => t.Kind == ThingKind.Comment));
            Assert.AreEqual(5, All(roots).Single(t => t.Kind == ThingKind.More).Data!.Count);
        }

        [TestMethod]
        public void Parse_ThrowsOnUnrecognisedMarkup()
        {
            const string html = "<shreddit-comment thingid=\"t1_x\" author=\"a\" depth=\"0\" created=\"2026-11-06T17:12:34.212000+0000\"></shreddit-comment>";

            Assert.ThrowsException<InvalidOperationException>(() => ShredditCommentParser.Parse(html, "t3_y", "sub"));
        }
    }
}
