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
        public async Task GetHot_RAll_ReturnsJsonNotHtml()
        {
            EnsureClientReady();

            var listing = await Client!.GetHotAsync("all", new ListingParameters { Limit = 5 });

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
        public async Task GetNew_RAll_ReturnsJsonNotHtml()
        {
            EnsureClientReady();

            var listing = await Client!.GetNewAsync("all", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task GetTop_RAll_ReturnsJsonNotHtml()
        {
            EnsureClientReady();

            var listing = await Client!.GetTopAsync("all", new ListingParameters { Limit = 5, Time = "day" });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task GetListing_RAll_ReturnsJsonNotHtml()
        {
            EnsureClientReady();

            var listing = await Client!.GetListingAsync<Link>("/r/all/hot", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task GetHot_RAll_WithBrowserToken_ReturnsJsonNotHtml()
        {
            // Simulate browser-token auth path: no OAuth credentials, token comes from refresh function
            var credentials = new Reddit.Api.Client.RedditCredentials
            {
                UserAgent = "Reddit.Api.Tests/1.0 (BrowserToken)"
            };

            using var browserClient = new Reddit.Api.Client.RedditClient(credentials);

            // Get a real OAuth token to simulate having a browser token
            using var http = new HttpClient();
            var authValue = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{TestSettings.AppKey}:{TestSettings.AppSecret}"));
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://www.reddit.com/api/v1/access_token");
            tokenRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);
            tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["username"] = TestSettings.Username!,
                ["password"] = TestSettings.Password!
            });
            tokenRequest.Headers.UserAgent.ParseAdd("Reddit.Api.Tests/1.0");
            var tokenResponse = await http.SendAsync(tokenRequest);
            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            var token = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(tokenJson)
                .GetProperty("access_token").GetString()!;

            browserClient.SetTokenRefreshFunction(() => Task.FromResult<string?>(token));

            var listing = await browserClient.GetHotAsync("all", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(listing);
            Assert.IsNotNull(listing.Data);
            Assert.IsNotNull(listing.Data.Children);
            Assert.IsTrue(listing.Data.Children.Count > 0);
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
