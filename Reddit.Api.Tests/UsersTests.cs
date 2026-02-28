using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class UsersTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task GetUserAbout_ReturnsUserInfo()
        {
            EnsureClientReady();

            // Use the authenticated user
            var username = AuthenticatedUsername!;

            var user = await Client!.GetUserAboutAsync(username);

            Assert.IsNotNull(user);
            Assert.AreEqual("t2", user.Kind);
            Assert.IsNotNull(user.Data);
            Assert.AreEqual(username, user.Data.Name, true);
        }

        [TestMethod]
        public async Task GetUserAbout_WithKnownUser_ReturnsInfo()
        {
            EnsureClientReady();

            // Use a well-known Reddit account
            var user = await Client!.GetUserAboutAsync("spez");

            Assert.IsNotNull(user);
            Assert.IsNotNull(user.Data);
            Assert.AreEqual("spez", user.Data.Name, true);
            Assert.IsTrue(user.Data.IsEmployee); // spez is a Reddit employee
        }

        [TestMethod]
        public async Task GetUserSubmitted_ReturnsPosts()
        {
            EnsureClientReady();

            var username = AuthenticatedUsername!;

            var posts = await Client!.GetUserSubmittedAsync(username, new ListingParameters { Limit = 5 });

            Assert.IsNotNull(posts);
            Assert.IsNotNull(posts.Data);
            // Posts may be empty for new accounts
        }

        [TestMethod]
        public async Task GetUserComments_ReturnsComments()
        {
            EnsureClientReady();

            var username = AuthenticatedUsername!;

            var comments = await Client!.GetUserCommentsAsync(username, new ListingParameters { Limit = 5 });

            Assert.IsNotNull(comments);
            Assert.IsNotNull(comments.Data);
            // Comments may be empty for new accounts
        }

        [TestMethod]
        public async Task GetUserOverview_ReturnsMixedContent()
        {
            EnsureClientReady();

            var username = AuthenticatedUsername!;

            var overview = await Client!.GetUserOverviewAsync(username, new ListingParameters { Limit = 5 });

            Assert.IsNotNull(overview);
            Assert.IsNotNull(overview.Data);
            // Overview may be empty for new accounts
        }

        [TestMethod]
        public async Task GetUserSaved_ReturnsSavedItems()
        {
            EnsureClientReady();

            var username = AuthenticatedUsername!;

            var saved = await Client!.GetUserSavedAsync(username, new ListingParameters { Limit = 5 });

            Assert.IsNotNull(saved);
            Assert.IsNotNull(saved.Data);
            // Saved items may be empty
        }

        [TestMethod]
        public async Task GetUserUpvoted_ReturnsUpvotedItems()
        {
            EnsureClientReady();

            var username = AuthenticatedUsername!;

            // This only works for own profile
            var upvoted = await Client!.GetUserUpvotedAsync(username, new ListingParameters { Limit = 5 });

            Assert.IsNotNull(upvoted);
            Assert.IsNotNull(upvoted.Data);
        }

        [TestMethod]
        public async Task GetUserDataByIds_ReturnsUserData()
        {
            EnsureClientReady();

            // First get a user's account ID
            var me = await Client!.GetMeAsync();
            Assert.IsNotNull(me);

            var accountId = $"t2_{me.Id}";

            var userData = await Client!.GetUserDataByIdsAsync(new[] { accountId });

            Assert.IsNotNull(userData);
            // Response may contain the user data
        }
    }
}
