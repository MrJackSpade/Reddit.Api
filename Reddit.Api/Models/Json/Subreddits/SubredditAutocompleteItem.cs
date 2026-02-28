using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Subreddits
{
    /// <summary>
    /// Simplified subreddit data returned by the autocomplete endpoint.
    /// </summary>
    public class SubredditAutocompleteItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("subscriber_count")]
        public int SubscriberCount { get; set; }

        [JsonPropertyName("active_user_count")]
        public int? ActiveUserCount { get; set; }

        [JsonPropertyName("icon_img")]
        public string? IconImg { get; set; }

        [JsonPropertyName("key_color")]
        public string? KeyColor { get; set; }

        [JsonPropertyName("community_icon")]
        public string? CommunityIcon { get; set; }

        [JsonPropertyName("allow_images")]
        public bool AllowImages { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("is_chat_post_feature_enabled")]
        public bool? IsChatPostFeatureEnabled { get; set; }

        /// <summary>
        /// Gets the display name (same as Name for autocomplete results).
        /// </summary>
        public string DisplayName => Name;
    }
}
