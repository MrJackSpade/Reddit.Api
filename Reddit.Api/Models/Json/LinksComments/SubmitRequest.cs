namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Request parameters for POST /api/submit.
    /// </summary>
    public class SubmitRequest
    {
        /// <summary>
        /// Subreddit name (without r/ prefix).
        /// </summary>
        public string Subreddit { get; set; } = string.Empty;

        /// <summary>
        /// Post title (up to 300 characters).
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Kind of submission: "self", "link", "image", "video", "videogif", "crosspost", "poll".
        /// </summary>
        public string Kind { get; set; } = "self";

        /// <summary>
        /// Raw markdown text for self posts.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// URL for link posts.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Enable/disable inbox replies (default true).
        /// </summary>
        public bool SendReplies { get; set; } = true;

        /// <summary>
        /// Resubmit if URL has already been posted.
        /// </summary>
        public bool? Resubmit { get; set; }

        /// <summary>
        /// Mark as NSFW.
        /// </summary>
        public bool? Nsfw { get; set; }

        /// <summary>
        /// Mark as spoiler.
        /// </summary>
        public bool? Spoiler { get; set; }

        /// <summary>
        /// Mark as original content.
        /// </summary>
        public bool? OriginalContent { get; set; }

        /// <summary>
        /// Flair ID to apply.
        /// </summary>
        public string? FlairId { get; set; }

        /// <summary>
        /// Flair text to apply.
        /// </summary>
        public string? FlairText { get; set; }

        /// <summary>
        /// Collection ID for collections.
        /// </summary>
        public string? CollectionId { get; set; }

        /// <summary>
        /// Discussion type for chat posts.
        /// </summary>
        public string? DiscussionType { get; set; }

        /// <summary>
        /// Fullname of the post to crosspost (for kind=crosspost).
        /// </summary>
        public string? CrosspostFullname { get; set; }

        /// <summary>
        /// Gallery items JSON for gallery posts.
        /// </summary>
        public string? Items { get; set; }

        /// <summary>
        /// The richtext_json (optional for RTE).
        /// </summary>
        public string? RichtextJson { get; set; }
    }

    /// <summary>
    /// Types of Reddit submissions.
    /// </summary>
    public static class SubmitKind
    {
        public const string Self = "self";
        public const string Link = "link";
        public const string Image = "image";
        public const string Video = "video";
        public const string VideoGif = "videogif";
        public const string Crosspost = "crosspost";
        public const string Poll = "poll";
    }
}
