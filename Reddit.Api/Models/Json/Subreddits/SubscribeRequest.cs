using Reddit.Api.Models.Enums;

namespace Reddit.Api.Models.Json.Subreddits
{
    /// <summary>
    /// Request parameters for POST /api/subscribe.
    /// </summary>
    public class SubscribeRequest
    {
        /// <summary>
        /// Action to take: sub or unsub.
        /// </summary>
        public SubscribeAction Action { get; set; } = SubscribeAction.Subscribe;

        /// <summary>
        /// Skip the initial frontpage for new users.
        /// </summary>
        public JsonBool SkipInitialDefaults { get; set; }

        /// <summary>
        /// Fullname (t5_xxx) or display name of the subreddit.
        /// </summary>
        public string Sr { get; set; } = string.Empty;

        /// <summary>
        /// For multi-subreddit subscription, comma-separated fullnames.
        /// </summary>
        public string? SrName { get; set; }
    }
}