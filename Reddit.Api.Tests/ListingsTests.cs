using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class ListingsTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task GetHot_ReturnsHotPosts()
        {
            EnsureClientReady();

            var listing = await Client!.GetHotAsync(parameters: new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);

            var firstPost = listing.Data.Children[0];
            Assert.AreEqual(ThingKind.Link, firstPost.Kind);
            Assert.IsNotNull(firstPost.Data);
            Assert.IsFalse(string.IsNullOrEmpty(firstPost.Data.Title));
        }

        [TestMethod]
        public async Task GetHot_WithSubreddit_ReturnsSubredditPosts()
        {
            EnsureClientReady();

            var listing = await Client!.GetHotAsync(TestSubreddit, new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);

            if (listing.Data.Children.Count > 0)
            {
                var firstPost = listing.Data.Children[0];
                Assert.IsNotNull(firstPost.Data);
                Assert.AreEqual(TestSubreddit, firstPost.Data.Subreddit, true);
            }
        }

        [TestMethod]
        public async Task GetNew_ReturnsNewPosts()
        {
            EnsureClientReady();

            var listing = await Client!.GetNewAsync(parameters: new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task GetTop_ReturnsTopPosts()
        {
            EnsureClientReady();

            var listing = await Client!.GetTopAsync(parameters: new ListingParameters { Limit = 5, Time = "day" });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
        }

        [TestMethod]
        public async Task GetControversial_ReturnsControversialPosts()
        {
            EnsureClientReady();

            var listing = await Client!.GetControversialAsync(parameters: new ListingParameters { Limit = 5, Time = "day" });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
        }

        [TestMethod]
        public async Task GetRising_ReturnsRisingPosts()
        {
            EnsureClientReady();

            var listing = await Client!.GetRisingAsync(parameters: new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
        }

        [TestMethod]
        public async Task GetComments_ReturnsPostAndComments()
        {
            EnsureClientReady();

            // First get a post from hot
            var listing = await Client!.GetHotAsync(TestSubreddit, new ListingParameters { Limit = 1 });
            Assert.IsNotNull(listing?.Data?.Children);

            if (listing.Data.Children.Count == 0)
            {
                Assert.Inconclusive("No posts available in test subreddit.");
                return;
            }

            var postId = listing.Data.Children[0].Data!.Id;

            var (post, comments) = await Client!.GetCommentsAsync(postId, limit: 10);

            Assert.IsNotNull(post);
            Assert.IsNotNull(post.Data);
            Assert.AreEqual(postId, post.Data.Id);
        }

        [TestMethod]
        public async Task GetBest_ReturnsBestPosts()
        {
            EnsureClientReady();

            var listing = await Client!.GetBestAsync(new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
        }

        [TestMethod]
        public async Task Pagination_WorksCorrectly()
        {
            EnsureClientReady();

            // Get first page
            var page1 = await Client!.GetHotAsync(parameters: new ListingParameters { Limit = 3 });
            Assert.IsNotNull(page1?.Data?.Children);
            Assert.IsTrue(page1.Data.Children.Count > 0);

            var afterToken = page1.Data.After;
            if (string.IsNullOrEmpty(afterToken))
            {
                Assert.Inconclusive("No pagination token available.");
                return;
            }

            // Get second page
            var page2 = await Client!.GetHotAsync(parameters: new ListingParameters { Limit = 3, After = afterToken });
            Assert.IsNotNull(page2?.Data?.Children);
            Assert.IsTrue(page2.Data.Children.Count > 0);

            // Verify different posts
            var page1Ids = page1.Data.Children.Select(c => c.Data!.Id).ToList();
            var page2Ids = page2.Data.Children.Select(c => c.Data!.Id).ToList();
            Assert.IsFalse(page1Ids.Intersect(page2Ids).Any(), "Pages should contain different posts");
        }
    }
}
