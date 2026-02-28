namespace Reddit.Api
{
    /// <summary>
    /// Credentials required for Reddit API authentication.
    /// </summary>
    public class RedditCredentials
    {
        /// <summary>
        /// Reddit username.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Reddit password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// OAuth2 client ID (from Reddit app registration).
        /// </summary>
        public string? AppKey { get; set; }

        /// <summary>
        /// OAuth2 client secret (from Reddit app registration).
        /// </summary>
        public string? AppSecret { get; set; }

        /// <summary>
        /// User-Agent header value (Reddit requires a descriptive User-Agent).
        /// </summary>
        public string UserAgent { get; set; } = "Reddit.Api/1.0";

        /// <summary>
        /// Returns true if all required credentials are provided.
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(AppKey) &&
            !string.IsNullOrWhiteSpace(AppSecret);
    }
}
