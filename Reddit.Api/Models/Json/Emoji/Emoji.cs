using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Emoji
{
    /// <summary>
    /// Response from GET /api/v1/{subreddit}/emojis/all.
    /// </summary>
    public class EmojisResponse
    {
        [JsonPropertyName("snoomojis")]
        public Dictionary<string, Emoji>? Snoomojis { get; set; }

        [JsonPropertyName("subreddit_emojis")]
        public Dictionary<string, Emoji>? SubredditEmojis { get; set; }
    }

    /// <summary>
    /// Individual emoji data.
    /// </summary>
    public class Emoji
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("created_by")]
        public string? CreatedBy { get; set; }

        [JsonPropertyName("mod_flair_only")]
        public bool ModFlairOnly { get; set; }

        [JsonPropertyName("post_flair_allowed")]
        public bool PostFlairAllowed { get; set; }

        [JsonPropertyName("user_flair_allowed")]
        public bool UserFlairAllowed { get; set; }
    }

    /// <summary>
    /// Request to add custom emoji.
    /// </summary>
    public class AddEmojiRequest
    {
        /// <summary>
        /// Emoji name (alphanumeric and underscores only).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// S3 image key from upload.
        /// </summary>
        public string S3Key { get; set; } = string.Empty;

        /// <summary>
        /// Whether only mods can use in flair.
        /// </summary>
        public bool ModFlairOnly { get; set; }

        /// <summary>
        /// Whether allowed in post flair.
        /// </summary>
        public bool PostFlairAllowed { get; set; } = true;

        /// <summary>
        /// Whether allowed in user flair.
        /// </summary>
        public bool UserFlairAllowed { get; set; } = true;
    }
}
