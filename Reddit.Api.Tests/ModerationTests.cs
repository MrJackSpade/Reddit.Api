using Reddit.Api.Models.Enums;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class ModerationTests : RedditClientTestBase
    {
        // Most moderation tests require moderator permissions
        // These tests are marked as Ignore and should be run manually
        // against a subreddit where the user is a moderator

        [TestMethod]
        public async Task GetModQueue_ReturnsItems()
        {
            EnsureClientReady();

            var modQueue = await Client!.GetModQueueAsync(TestSubreddit);

            Assert.IsNotNull(modQueue);
            Assert.IsNotNull(modQueue.Data);
        }

        [TestMethod]
        public async Task GetReports_ReturnsReportedItems()
        {
            EnsureClientReady();

            var reports = await Client!.GetReportsAsync(TestSubreddit);

            Assert.IsNotNull(reports);
            Assert.IsNotNull(reports.Data);
        }

        [TestMethod]
        public async Task GetSpam_ReturnsSpamItems()
        {
            EnsureClientReady();

            var spam = await Client!.GetSpamAsync(TestSubreddit);

            Assert.IsNotNull(spam);
            Assert.IsNotNull(spam.Data);
        }

        [TestMethod]
        public async Task GetModLog_ReturnsActions()
        {
            EnsureClientReady();

            var log = await Client!.GetModLogAsync(TestSubreddit);

            Assert.IsNotNull(log);
            Assert.IsNotNull(log.Data);
        }

        [TestMethod]
        public async Task Approve_And_Remove_Works()
        {
            EnsureClientReady();

            // Create a test post to work with
            var submitRequest = new Reddit.Api.Models.Json.LinksComments.SubmitRequest
            {
                Subreddit = TestSubreddit,
                Title = $"Mod Test Post {DateTime.UtcNow:O}",
                Kind = SubmitKind.Self,
                Text = "This is a test post for moderation testing.",
                SendReplies = false
            };

            var post = await Client!.SubmitAsync(submitRequest);
            Assert.IsNotNull(post);
            Assert.IsFalse(string.IsNullOrEmpty(post.Name));

            try
            {
                // Remove the post
                var removeResult = await Client!.RemoveAsync(post.Name);
                Assert.IsTrue(removeResult);

                // Approve the post
                var approveResult = await Client!.ApproveAsync(post.Name);
                Assert.IsTrue(approveResult);
            }
            finally
            {
                // Clean up - delete the post
                await Client!.DeleteThingAsync(post.Name);
            }
        }

        [TestMethod]
        public async Task Distinguish_Works()
        {
            EnsureClientReady();

            // Create a test post first
            var submitRequest = new Reddit.Api.Models.Json.LinksComments.SubmitRequest
            {
                Subreddit = TestSubreddit,
                Title = $"Distinguish Test Post {DateTime.UtcNow:O}",
                Kind = SubmitKind.Self,
                Text = "This is a test post for distinguish testing.",
                SendReplies = false
            };

            var post = await Client!.SubmitAsync(submitRequest);
            Assert.IsNotNull(post);

            try
            {
                // Add a comment as the mod
                var comment = await Client!.CommentAsync(post.Name, "This is a mod comment for testing distinguish.");
                Assert.IsNotNull(comment);
                Assert.IsNotNull(comment.Data);

                // Distinguish the comment
                var distinguished = await Client!.DistinguishAsync(comment.Data.Name, DistinguishHow.Yes);
                Assert.IsNotNull(distinguished);

                // Undistinguish
                var undistinguished = await Client!.DistinguishAsync(comment.Data.Name, DistinguishHow.No);
                Assert.IsNotNull(undistinguished);
            }
            finally
            {
                // Clean up - delete the post (and its comments)
                await Client!.DeleteThingAsync(post.Name);
            }
        }
    }
}
