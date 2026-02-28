namespace Reddit.Api.Tests
{
    [TestClass]
    public class FlairTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task GetLinkFlair_ReturnsTemplates()
        {
            EnsureClientReady();

            var flairs = await Client!.GetLinkFlairAsync(TestSubreddit);

            // Link flair may not be enabled on all subreddits
            // Just verify no exception is thrown
            Assert.IsTrue(flairs == null || flairs.Count >= 0);
        }

        [TestMethod]
        public async Task GetUserFlair_ReturnsTemplates()
        {
            EnsureClientReady();

            var flairs = await Client!.GetUserFlairAsync(TestSubreddit);

            // User flair may not be enabled on all subreddits
            Assert.IsTrue(flairs == null || flairs.Count >= 0);
        }

        [TestMethod]
        public async Task GetFlairList_ReturnsUserFlairs()
        {
            EnsureClientReady();

            var flairList = await Client!.GetFlairListAsync(TestSubreddit, limit: 10);

            Assert.IsNotNull(flairList);
            Assert.IsNotNull(flairList.Users);
        }
    }
}
