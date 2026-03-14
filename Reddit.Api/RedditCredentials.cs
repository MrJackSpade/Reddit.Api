namespace Reddit.Api.Client
{
    /// <summary>
    /// Credentials required for Reddit API authentication.
    /// Used internally by the new Reddit.Api.Client.RedditClient.
    /// </summary>
    public class RedditCredentials
    {
        /// <summary>
        /// OAuth2 client ID (from Reddit app registration).
        /// </summary>
        public string? AppKey { get; set; }

        /// <summary>
        /// OAuth2 client secret (from Reddit app registration).
        /// </summary>
        public string? AppSecret { get; set; }

        /// <summary>
        /// Returns true if all required credentials are provided.
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(AppKey) &&
            !string.IsNullOrWhiteSpace(AppSecret);

        /// <summary>
        /// Reddit password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// User-Agent header value (Reddit requires a descriptive User-Agent).
        /// </summary>
        public string UserAgent { get; set; } = "Reddit.Api/1.0";

        /// <summary>
        /// Reddit username.
        /// </summary>
        public string? Username { get; set; }
    }
}