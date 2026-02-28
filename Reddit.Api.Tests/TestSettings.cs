using Microsoft.Extensions.Configuration;
using Reddit.Api.Client;

namespace Reddit.Api.Tests
{
    /// <summary>
    /// Provides test configuration from user secrets.
    /// </summary>
    public static class TestSettings
    {
        private static IConfiguration? _configuration;

        public static IConfiguration Configuration => _configuration ??= new ConfigurationBuilder()
            .AddUserSecrets<RedditClientTestBase>()
            .Build();

        public static string? Username => Configuration["Reddit:Username"];
        public static string? Password => Configuration["Reddit:Password"];
        public static string? AppKey => Configuration["Reddit:AppKey"];
        public static string? AppSecret => Configuration["Reddit:AppSecret"];
        public static string TestSubreddit => Configuration["Reddit:TestSubreddit"] ?? "test";

        public static bool HasValidCredentials =>
            !string.IsNullOrEmpty(Username) &&
            !string.IsNullOrEmpty(Password) &&
            !string.IsNullOrEmpty(AppKey) &&
            !string.IsNullOrEmpty(AppSecret);

        public static RedditCredentials GetCredentials() => new()
        {
            Username = Username,
            Password = Password,
            AppKey = AppKey,
            AppSecret = AppSecret,
            UserAgent = "Reddit.Api.Tests/1.0 (by /u/test)"
        };
    }
}
