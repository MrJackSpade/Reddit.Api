using Reddit.Api.Client;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.LinksComments;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class RenderedThingParserTests
    {
        private static MoreChildrenThingData Fixture() => new()
        {
            Id = "t1_19s3i9cbma3qa",
            Parent = "t1_19s3i3tahaezp",
            // Captured from old.reddit.com/api/morechildren (no .json suffix).
            Content = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "morechildren-more.html"))
        };

        private static MoreChildrenThingData CommentFixture() => new()
        {
            Id = "t1_19s3g263ezs6b",
            Parent = "t1_19s3esl34wrj7",
            Link = "t3_1xn0g2v",
            ContentText = "Example body.",
            ContentHtml = "<div class=\"md\"><p>Example body.</p>\n</div>",
            // Captured from old.reddit.com/api/morechildren, with the author, text and score replaced.
            Content = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "morechildren-comment.html"))
        };

        [TestMethod]
        public void ParseComment_ReadsFieldsFromMarkup()
        {
            Comment comment = RenderedThingParser.ParseComment(CommentFixture());

            Assert.AreEqual("19s3g263ezs6b", comment.Id);
            Assert.AreEqual("t1_19s3g263ezs6b", comment.Name);
            Assert.AreEqual("t1_19s3esl34wrj7", comment.ParentId);
            Assert.AreEqual("t3_1xn0g2v", comment.LinkId);
            Assert.AreEqual("example_author", comment.Author);
            Assert.AreEqual("news", comment.Subreddit);
            Assert.AreEqual("/r/news/comments/1xn0g2v/at_least_33_dead_after_russia_drops_nine_bombs_on/19s3g263ezs6b/", comment.Permalink);
            Assert.AreEqual("Example body.", comment.Body);
            Assert.AreEqual(7, comment.Score);
            Assert.AreEqual(VoteState.None, comment.Likes);
            Assert.AreEqual(DateTimeOffset.Parse("2026-11-03T20:46:17+00:00").UtcDateTime, comment.CreatedUtc.Value);
            Assert.IsTrue(comment.Edited.HasValue);
        }

        [TestMethod]
        public void ParseComment_UsesTheScoreOfTheCurrentVoteState()
        {
            MoreChildrenThingData data = CommentFixture();
            data.Content = data.Content.Replace("entry unvoted", "entry likes");

            Comment comment = RenderedThingParser.ParseComment(data);

            Assert.AreEqual(VoteState.Upvote, comment.Likes);
            Assert.AreEqual(8, comment.Score);
        }

        [TestMethod]
        public void ParseComment_DeletedHasNoAuthorOrScore()
        {
            MoreChildrenThingData data = new()
            {
                Id = "t1_19s3co5wbgiyr",
                Parent = "t1_19s3caumtaxvm",
                Link = "t3_1xn2byi",
                ContentText = "[removed]",
                // Captured from old.reddit.com/api/morechildren.
                Content = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "morechildren-deleted.html"))
            };

            Comment comment = RenderedThingParser.ParseComment(data);

            Assert.AreEqual("[deleted]", comment.Author);
            Assert.AreEqual(0, comment.Score);
            Assert.AreEqual(CollapsedReasonCode.Deleted, comment.CollapsedReasonCode);
            Assert.AreEqual("/r/PoursTea/comments/1xn2byi/an_accurate_prediction/19s3co5wbgiyr/", comment.Permalink);
        }

        [TestMethod]
        public void ParseComment_MissingTimeThrows()
        {
            MoreChildrenThingData data = CommentFixture();
            data.Content = data.Content.Replace("live-timestamp", "other");

            Assert.ThrowsException<InvalidOperationException>(() => RenderedThingParser.ParseComment(data));
        }

        [TestMethod]
        public void Parse_MoreThing_ReturnsMoreKind()
        {
            Thing<Comment> thing = RenderedThingParser.Parse(new MoreChildrenThing { Kind = ThingKind.More, Data = Fixture() }, "t3_x");

            Assert.AreEqual(ThingKind.More, thing.Kind);
            Assert.AreEqual(2, thing.Data!.Children!.Count);
        }

        [TestMethod]
        public void Parse_UnexpectedKind_Throws()
        {
            Assert.ThrowsException<InvalidOperationException>(() => RenderedThingParser.Parse(new MoreChildrenThing { Kind = ThingKind.Link, Data = Fixture() }, "t3_x"));
        }

        [TestMethod]
        public void Parse_LoadMore_ReadsChildrenCountAndIdentity()
        {
            Comment more = RenderedThingParser.ParseMore(Fixture());

            Assert.AreEqual("19s3i9cbma3qa", more.Id);
            Assert.AreEqual("t1_19s3i9cbma3qa", more.Name);
            Assert.AreEqual("t1_19s3i3tahaezp", more.ParentId);
            CollectionAssert.AreEqual(new[] { "19s3i9cbma3qa", "19s3iax2l8go1" }, more.Children);
            Assert.AreEqual(4, more.Count);
        }

        [TestMethod]
        public void Parse_ContinueThisThread_HasNoChildren()
        {
            MoreChildrenThingData data = new()
            {
                Id = "t1__",
                Parent = "t1_19s3i319ys95u",
                // Captured from old.reddit.com/api/morechildren (no .json suffix).
                Content = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "morechildren-continue-thread.html"))
            };

            Comment more = RenderedThingParser.ParseMore(data);

            // Reddit's JSON gives "continue this thread" placeholders id "_" and no children.
            Assert.AreEqual("_", more.Id);
            Assert.AreEqual("t1__", more.Name);
            Assert.AreEqual("t1_19s3i319ys95u", more.ParentId);
            Assert.AreEqual(0, more.Children!.Count);
            Assert.AreEqual(0, more.Count);
        }

        [TestMethod]
        public void Parse_UnrecognisedMarkup_Throws()
        {
            MoreChildrenThingData data = new()
            {
                Id = "t1_abc",
                Parent = "t1_parent",
                Content = "<div class=\"thing\" data-fullname=\"t1_abc\"><div class=\"entry\"></div></div>"
            };

            Assert.ThrowsException<InvalidOperationException>(() => RenderedThingParser.ParseMore(data));
        }

        [TestMethod]
        public void Parse_MismatchedFullname_Throws()
        {
            MoreChildrenThingData data = Fixture();
            data.Id = "t1_other";

            Assert.ThrowsException<InvalidOperationException>(() => RenderedThingParser.ParseMore(data));
        }
    }
}
