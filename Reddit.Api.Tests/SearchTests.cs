using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Search;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class SearchTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task Search_WithQuery_ReturnsResults()
        {
            EnsureClientReady();

            var parameters = new SearchParameters
            {
                Query = "programming",
                Limit = 10,
                Sort = SearchSort.Relevance
            };

            var results = await Client!.SearchAsync(parameters);

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Data);
            Assert.IsNotNull(results.Data.Children);
            Assert.IsTrue(results.Data.Children.Count > 0);
        }

        [TestMethod]
        public async Task Search_WithTimeFilter_ReturnsFilteredResults()
        {
            EnsureClientReady();

            var parameters = new SearchParameters
            {
                Query = "news",
                Limit = 5,
                Sort = SearchSort.Top,
                Time = "day"
            };

            var results = await Client!.SearchAsync(parameters);

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Data);
        }

        [TestMethod]
        public async Task SearchSubreddit_ReturnsSubredditResults()
        {
            EnsureClientReady();

            var parameters = new SearchParameters
            {
                Query = "help",
                Limit = 5
            };

            var results = await Client!.SearchSubredditAsync(TestSubreddit, parameters);

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Data);

            // All results should be from the specified subreddit
            if (results.Data.Children?.Count > 0)
            {
                foreach (var child in results.Data.Children)
                {
                    Assert.AreEqual(TestSubreddit, child.Data!.Subreddit, true);
                }
            }
        }

        [TestMethod]
        public async Task Search_Pagination_Works()
        {
            EnsureClientReady();

            // First page
            var page1Params = new SearchParameters
            {
                Query = "reddit",
                Limit = 5
            };

            var page1 = await Client!.SearchAsync(page1Params);
            Assert.IsNotNull(page1?.Data);

            if (string.IsNullOrEmpty(page1.Data.After))
            {
                Assert.Inconclusive("No pagination available in results.");
                return;
            }

            // Second page
            var page2Params = new SearchParameters
            {
                Query = "reddit",
                Limit = 5,
                After = page1.Data.After
            };

            var page2 = await Client!.SearchAsync(page2Params);
            Assert.IsNotNull(page2?.Data);
            Assert.IsNotNull(page2.Data.Children);

            // Verify different results
            if (page1.Data.Children?.Count > 0 && page2.Data.Children.Count > 0)
            {
                var page1Ids = page1.Data.Children.Select(c => c.Data!.Id).ToList();
                var page2Ids = page2.Data.Children.Select(c => c.Data!.Id).ToList();
                Assert.IsFalse(page1Ids.Intersect(page2Ids).Any(), "Pages should contain different results");
            }
        }
    }
}
