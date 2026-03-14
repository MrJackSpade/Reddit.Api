namespace Reddit.Api.Tests
{
    [TestClass]
    public class AccountTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task GetMe_ReturnsCurrentUserIdentity()
        {
            EnsureClientReady();

            var me = await Client!.GetMeAsync();

            Assert.IsNotNull(me);
            Assert.IsFalse(string.IsNullOrEmpty(me.Name));
            Assert.IsFalse(string.IsNullOrEmpty(me.Id));
            Assert.AreEqual(TestSettings.Username, me.Name, true);
        }

        [TestMethod]
        public async Task GetMyKarma_ReturnsKarmaBreakdown()
        {
            EnsureClientReady();

            var karma = await Client!.GetMyKarmaAsync();

            Assert.IsNotNull(karma);
            Assert.IsNotNull(karma.Data);
            // Karma list can be empty for new accounts
        }

        [TestMethod]
        public async Task GetMyPrefs_ReturnsUserPreferences()
        {
            EnsureClientReady();

            var prefs = await Client!.GetMyPrefsAsync();

            Assert.IsNotNull(prefs);
            // Check some common preferences exist
            Assert.IsNotNull(prefs.Lang);
        }

        [TestMethod]
        public async Task GetMyTrophies_ReturnsTrophyList()
        {
            EnsureClientReady();

            var trophies = await Client!.GetMyTrophiesAsync();

            Assert.IsNotNull(trophies);
            Assert.IsNotNull(trophies.Data);
            // Trophy list can be empty but should exist
        }

        [TestMethod]
        public async Task GetMyFriends_ReturnsFriendsList()
        {
            EnsureClientReady();

            var friends = await Client!.GetMyFriendsAsync();

            Assert.IsNotNull(friends);
            // Friends list may be empty but response should be valid
        }

        [TestMethod]
        public async Task GetMyBlocked_ReturnsBlockedList()
        {
            EnsureClientReady();

            var blocked = await Client!.GetMyBlockedAsync();

            Assert.IsNotNull(blocked);
            // Blocked list may be empty but response should be valid
        }
    }
}
