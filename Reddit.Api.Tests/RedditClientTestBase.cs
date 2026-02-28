using Reddit.Api;
using Reddit.Api.Client;

namespace Reddit.Api.Tests
{
    /// <summary>
    /// Base class for Reddit API client tests.
    /// Provides authenticated client and test utilities.
    /// </summary>
    [TestClass]
    public abstract class RedditClientTestBase
    {
        protected static Client.RedditClient? Client { get; private set; }
        protected static string TestSubreddit => TestSettings.TestSubreddit;
        protected static string? AuthenticatedUsername => Client?.AuthenticatedUser;

        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext context)
        {
            if (!TestSettings.HasValidCredentials)
            {
                Assert.Inconclusive("Reddit API credentials not configured. Run: dotnet user-secrets init && dotnet user-secrets set \"Reddit:Username\" \"your_username\" etc.");
                return;
            }

            Client = new Client.RedditClient(TestSettings.GetCredentials());
            var authenticated = await Client.AuthenticateAsync();

            if (!authenticated)
            {
                Assert.Inconclusive("Failed to authenticate with Reddit API. Check credentials.");
            }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Client?.Dispose();
        }

        /// <summary>
        /// Ensures the client is available and authenticated.
        /// </summary>
        protected void EnsureClientReady()
        {
            if (Client == null || !Client.IsAuthenticated)
            {
                Assert.Inconclusive("Client not available or not authenticated.");
            }
        }

        /// <summary>
        /// Skips test if not authenticated.
        /// </summary>
        protected void RequireAuthentication()
        {
            if (!TestSettings.HasValidCredentials)
            {
                Assert.Inconclusive("Test requires Reddit API credentials.");
            }
        }
    }
}
