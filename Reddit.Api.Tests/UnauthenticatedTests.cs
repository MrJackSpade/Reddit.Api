using Reddit.Api.Client;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class UnauthenticatedTests
    {
        private Client.RedditClient _client = null!;

        [TestInitialize]
        public void Setup()
        {
            // Create client with empty/invalid credentials - should not authenticate
            var credentials = new RedditCredentials
            {
                UserAgent = "Reddit.Api.Tests/1.0 (Unauthenticated)"
            };
            _client = new Client.RedditClient(credentials);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client?.Dispose();
        }

        [TestMethod]
        public void Client_WithoutCredentials_IsNotAuthenticated()
        {
            Assert.IsFalse(_client.CanAuthenticate);
            Assert.IsFalse(_client.IsAuthenticated);
        }

        [TestMethod]
        public async Task GetHot_WithoutAuth_ReturnsPublicPosts()
        {
            var listing = await _client.GetHotAsync("AskReddit", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task GetNew_WithoutAuth_ReturnsPublicPosts()
        {
            var listing = await _client.GetNewAsync("programming", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task GetTop_WithoutAuth_ReturnsPublicPosts()
        {
            var listing = await _client.GetTopAsync("funny", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task GetSubredditAbout_WithoutAuth_ReturnsPublicInfo()
        {
            var subreddit = await _client.GetSubredditAboutAsync("AskReddit");

            Assert.IsNotNull(subreddit);
            Assert.AreEqual(ThingKind.Subreddit, subreddit.Kind);
            Assert.IsNotNull(subreddit.Data);
            Assert.AreEqual("AskReddit", subreddit.Data.DisplayName, true);
        }

        [TestMethod]
        public async Task GetComments_WithoutAuth_ReturnsPublicComments()
        {
            // First get a post ID from hot
            var listing = await _client.GetHotAsync("AskReddit", new ListingParameters { Limit = 1 });
            Assert.IsNotNull(listing?.Data?.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);

            var postId = listing.Data.Children[0].Data!.Id;

            // Get comments for that post
            var (post, comments) = await _client.GetCommentsAsync(postId);

            Assert.IsNotNull(post);
            Assert.IsNotNull(comments);
        }

        [TestMethod]
        public async Task SearchSubreddits_WithoutAuth_ReturnsResults()
        {
            var results = await _client.SearchSubredditsAsync("programming", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Data);
            Assert.IsNotNull(results.Data.Children);
            Assert.IsTrue(results.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task GetUserAbout_WithoutAuth_ReturnsPublicUserInfo()
        {
            var user = await _client.GetUserAboutAsync("spez");

            Assert.IsNotNull(user);
            Assert.AreEqual(ThingKind.Account, user.Kind);
            Assert.IsNotNull(user.Data);
            Assert.AreEqual("spez", user.Data.Name, true);
        }

        [TestMethod]
        public async Task GetUserSubmitted_WithoutAuth_ReturnsPublicPosts()
        {
            var posts = await _client.GetUserSubmittedAsync("spez", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(posts);
            Assert.IsNotNull(posts.Data);
        }
    }
}
