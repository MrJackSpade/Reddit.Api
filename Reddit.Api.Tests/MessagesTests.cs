using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class MessagesTests : RedditClientTestBase
    {
        [TestMethod]
        public async Task GetInbox_ReturnsInboxMessages()
        {
            EnsureClientReady();

            var inbox = await Client!.GetInboxAsync(new ListingParameters { Limit = 5 });

            Assert.IsNotNull(inbox);
            Assert.IsNotNull(inbox.Data);
            // Inbox may be empty
        }

        [TestMethod]
        public async Task GetSent_ReturnsSentMessages()
        {
            EnsureClientReady();

            var sent = await Client!.GetSentAsync(new ListingParameters { Limit = 5 });

            Assert.IsNotNull(sent);
            Assert.IsNotNull(sent.Data);
            // Sent may be empty
        }

        [TestMethod]
        public async Task GetUnread_ReturnsUnreadMessages()
        {
            EnsureClientReady();

            var unread = await Client!.GetUnreadAsync(new ListingParameters { Limit = 5 });

            Assert.IsNotNull(unread);
            Assert.IsNotNull(unread.Data);
            // Unread may be empty
        }

        [TestMethod]
        public async Task ReadAllMessages_Succeeds()
        {
            EnsureClientReady();

            var result = await Client!.ReadAllMessagesAsync();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ComposeMessage_SendsMessage()
        {
            EnsureClientReady();

            var result = await Client!.ComposeMessageAsync(
                to: "PhoenixModBot",
                subject: "Test Message",
                body: "This is an automated test message from Reddit.Api.Tests.");

            Assert.IsTrue(result);
        }
    }
}
