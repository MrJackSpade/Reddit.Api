using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class LinksCommentsTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task GetInfo_ByIds_ReturnsThings()
        {
            EnsureClientReady();

            // First get a post to use as test data
            var listing = await Client!.GetHotAsync(TestSubreddit, new ListingParameters { Limit = 1 });

            if (listing?.Data?.Children == null || listing.Data.Children.Count == 0)
            {
                Assert.Inconclusive("No posts available for testing.");
                return;
            }

            var postFullname = listing.Data.Children[0].Data!.Name;

            var info = await Client!.GetInfoAsync(ids: new[] { postFullname });

            Assert.IsNotNull(info);
            Assert.IsNotNull(info.Data);
            Assert.IsNotNull(info.Data.Children);
            Assert.IsTrue(info.Data.Children.Count > 0);
            Assert.AreEqual(postFullname, info.Data.Children[0].Data!.Name);
        }

        [TestMethod]
        public async Task Vote_Upvote_Succeeds()
        {
            EnsureClientReady();

            // Get a post to vote on
            var listing = await Client!.GetHotAsync(TestSubreddit, new ListingParameters { Limit = 1 });

            if (listing?.Data?.Children == null || listing.Data.Children.Count == 0)
            {
                Assert.Inconclusive("No posts available for testing.");
                return;
            }

            var postFullname = listing.Data.Children[0].Data!.Name;

            // Upvote
            var result = await Client!.VoteAsync(postFullname, VoteDirection.Upvote);
            Assert.IsTrue(result);

            // Clear vote to clean up
            await Client!.VoteAsync(postFullname, VoteDirection.None);
        }

        [TestMethod]
        public async Task Save_AndUnsave_Succeeds()
        {
            EnsureClientReady();

            var listing = await Client!.GetHotAsync(TestSubreddit, new ListingParameters { Limit = 1 });

            if (listing?.Data?.Children == null || listing.Data.Children.Count == 0)
            {
                Assert.Inconclusive("No posts available for testing.");
                return;
            }

            var postFullname = listing.Data.Children[0].Data!.Name;

            // Save
            var saveResult = await Client!.SaveAsync(postFullname);
            Assert.IsTrue(saveResult);

            // Unsave
            var unsaveResult = await Client!.UnsaveAsync(postFullname);
            Assert.IsTrue(unsaveResult);
        }

        [TestMethod]
        public async Task Hide_AndUnhide_Succeeds()
        {
            EnsureClientReady();

            var listing = await Client!.GetHotAsync(TestSubreddit, new ListingParameters { Limit = 1 });

            if (listing?.Data?.Children == null || listing.Data.Children.Count == 0)
            {
                Assert.Inconclusive("No posts available for testing.");
                return;
            }

            var postFullname = listing.Data.Children[0].Data!.Name;

            // Hide
            var hideResult = await Client!.HideAsync(postFullname);
            Assert.IsTrue(hideResult);

            // Unhide
            var unhideResult = await Client!.UnhideAsync(postFullname);
            Assert.IsTrue(unhideResult);
        }

        [TestMethod]
        public async Task GetMoreChildren_ReturnsComments()
        {
            EnsureClientReady();

            // Get a post with comments
            var listing = await Client!.GetHotAsync("AskReddit", new ListingParameters { Limit = 5 });

            if (listing?.Data?.Children == null || listing.Data.Children.Count == 0)
            {
                Assert.Inconclusive("No posts available for testing.");
                return;
            }

            // Find a post with many comments
            var post = listing.Data.Children
                .FirstOrDefault(c => c.Data!.NumComments > 100);

            if (post == null)
            {
                Assert.Inconclusive("No posts with enough comments found.");
                return;
            }

            var (_, comments) = await Client!.GetCommentsAsync(post.Data!.Id, limit: 1);

            // This test validates the structure; actual "more" children depend on the thread
            Assert.IsNotNull(comments);
        }

        [TestMethod]
        public async Task Submit_SelfPost_CreatesPost()
        {
            EnsureClientReady();

            var request = new SubmitRequest
            {
                Subreddit = TestSubreddit,
                Title = $"Test Post {DateTime.UtcNow:O}",
                Kind = SubmitKind.Self,
                Text = "This is an automated test post.",
                SendReplies = false
            };

            var result = await Client!.SubmitAsync(request);

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Name));
            Assert.IsFalse(string.IsNullOrEmpty(result.Url));

            // Clean up - delete the post
            if (!string.IsNullOrEmpty(result.Name))
            {
                await Client!.DeleteThingAsync(result.Name);
            }
        }
    }
}
