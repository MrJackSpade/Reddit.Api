namespace Reddit.Api.Models.Json.Messages
{
    /// <summary>
    /// Request parameters for POST /api/compose.
    /// </summary>
    public class ComposeRequest
    {
        /// <summary>
        /// Subreddit to send from (for moderator messages).
        /// </summary>
        public string? FromSr { get; set; }

        /// <summary>
        /// UUID to track the message.
        /// </summary>
        public string? G_recaptcha_response { get; set; }

        /// <summary>
        /// The richtext_json (optional for RTE).
        /// </summary>
        public string? RichtextJson { get; set; }

        /// <summary>
        /// Message subject (up to 100 characters).
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Message body in markdown.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Recipient username or /r/subreddit for modmail.
        /// </summary>
        public string To { get; set; } = string.Empty;
    }
}