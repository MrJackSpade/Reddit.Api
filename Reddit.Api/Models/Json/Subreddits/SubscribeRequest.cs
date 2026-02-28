namespace Reddit.Api.Models.Json.Subreddits
{
    /// <summary>
    /// Request parameters for POST /api/subscribe.
    /// </summary>
    public class SubscribeRequest
    {
        /// <summary>
        /// Action to take: "sub" or "unsub".
        /// </summary>
        public string Action { get; set; } = "sub";

        /// <summary>
        /// Fullname (t5_xxx) or display name of the subreddit.
        /// </summary>
        public string Sr { get; set; } = string.Empty;

        /// <summary>
        /// For multi-subreddit subscription, comma-separated fullnames.
        /// </summary>
        public string? SrName { get; set; }

        /// <summary>
        /// Skip the initial frontpage for new users.
        /// </summary>
        public bool? SkipInitialDefaults { get; set; }
    }

    /// <summary>
    /// Subscribe action types.
    /// </summary>
    public static class SubscribeAction
    {
        public const string Subscribe = "sub";
        public const string Unsubscribe = "unsub";
    }
}
