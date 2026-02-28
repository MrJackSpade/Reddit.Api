using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class SubredditsTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task GetSubredditAbout_ReturnsSubredditInfo()
        {
            EnsureClientReady();

            var subreddit = await Client!.GetSubredditAboutAsync(TestSubreddit);

            Assert.IsNotNull(subreddit);
            Assert.AreEqual(ThingKind.Subreddit, subreddit.Kind);
            Assert.IsNotNull(subreddit.Data);
            Assert.AreEqual(TestSubreddit, subreddit.Data.DisplayName, true);
            Assert.IsFalse(string.IsNullOrEmpty(subreddit.Data.Id));
        }

        [TestMethod]
        public async Task GetSubredditRules_ReturnsRules()
        {
            EnsureClientReady();

            var rules = await Client!.GetSubredditRulesAsync(TestSubreddit);

            Assert.IsNotNull(rules);
            // Rules may be empty but response should be valid
            Assert.IsNotNull(rules.SiteRules);
        }

        [TestMethod]
        public async Task GetMySubscribed_ReturnsSubscribedSubreddits()
        {
            EnsureClientReady();

            var subscribed = await Client!.GetMySubscribedAsync(new ListingParameters { Limit = 10 });

            Assert.IsNotNull(subscribed);
            Assert.IsNotNull(subscribed.Data);
            // User should have at least some subscriptions
        }

        [TestMethod]
        public async Task SearchSubreddits_ReturnsResults()
        {
            EnsureClientReady();

            var results = await Client!.SearchSubredditsAsync("programming", new ListingParameters { Limit = 5 });

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Data);
            Assert.IsNotNull(results.Data.Children);
            Assert.IsTrue(results.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task AutocompleteSubreddits_ReturnsMatches()
        {
            EnsureClientReady();

            var results = await Client!.AutocompleteSubredditsAsync("prog");

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.Any(s => s.Name.Contains("prog", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public async Task Subscribe_AndUnsubscribe_Succeeds()
        {
            EnsureClientReady();

            // Subscribe
            var subscribeResult = await Client!.SubscribeAsync(TestSubreddit, true);
            Assert.IsTrue(subscribeResult);

            // Unsubscribe
            var unsubscribeResult = await Client!.SubscribeAsync(TestSubreddit, false);
            Assert.IsTrue(unsubscribeResult);
        }

        [TestMethod]
        public async Task GetPostRequirements_ReturnsRequirements()
        {
            EnsureClientReady();

            var requirements = await Client!.GetPostRequirementsAsync(TestSubreddit);

            // PostRequirements may return null for some subreddits
            // Just verify no exception is thrown
        }
    }
}
