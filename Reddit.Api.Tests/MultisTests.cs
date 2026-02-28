namespace Reddit.Api.Tests
{
    [TestClass]
    public class MultisTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task GetMyMultis_ReturnsMultireddits()
        {
            EnsureClientReady();

            var multis = await Client!.GetMyMultisAsync();

            Assert.IsNotNull(multis);
            // Multis may be empty but should be valid list
        }

        [TestMethod]
        public async Task GetUserMultis_WithKnownUser_ReturnsPublicMultis()
        {
            EnsureClientReady();

            // Some users have public multireddits
            var multis = await Client!.GetUserMultisAsync(AuthenticatedUsername!);

            Assert.IsNotNull(multis);
            // Public multis may be empty
        }

        [TestMethod]
        public async Task CreateAndDeleteMulti_Succeeds()
        {
            EnsureClientReady();

            var multipath = $"user/{AuthenticatedUsername}/m/test_multi";
            var request = new Reddit.Api.Models.Json.Multis.MultiCreateRequest
            {
                DisplayName = "Test Multi",
                DescriptionMd = "Automated test multi",
                Visibility = "private",
                Subreddits = new()
                {
                    new() { Name = TestSubreddit }
                }
            };

            // Create
            var created = await Client!.CreateOrUpdateMultiAsync(multipath, request);
            Assert.IsNotNull(created);
            Assert.IsNotNull(created.Data);

            // Delete
            var deleted = await Client!.DeleteMultiAsync(multipath);
            Assert.IsTrue(deleted);
        }
    }
}
