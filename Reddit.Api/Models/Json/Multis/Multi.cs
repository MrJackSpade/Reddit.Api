using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Multis
{
    /// <summary>
    /// Response wrapper for multi operations.
    /// </summary>
    public class MultiResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "LabeledMulti";

        [JsonPropertyName("data")]
        public Multi? Data { get; set; }
    }

    /// <summary>
    /// Reddit multireddit data.
    /// </summary>
    public class Multi
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("description_md")]
        public string? DescriptionMd { get; set; }

        [JsonPropertyName("description_html")]
        public string? DescriptionHtml { get; set; }

        [JsonPropertyName("key_color")]
        public string? KeyColor { get; set; }

        [JsonPropertyName("icon_url")]
        public string? IconUrl { get; set; }

        [JsonPropertyName("visibility")]
        public string Visibility { get; set; } = string.Empty;

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; } = string.Empty;

        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; } = string.Empty;

        [JsonPropertyName("can_edit")]
        public bool CanEdit { get; set; }

        [JsonPropertyName("is_subscriber")]
        public bool? IsSubscriber { get; set; }

        [JsonPropertyName("is_favorited")]
        public bool? IsFavorited { get; set; }

        [JsonPropertyName("num_subscribers")]
        public int NumSubscribers { get; set; }

        [JsonPropertyName("copied_from")]
        public string? CopiedFrom { get; set; }

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonPropertyName("subreddits")]
        public List<MultiSubreddit> Subreddits { get; set; } = [];
    }

    /// <summary>
    /// Subreddit within a multireddit.
    /// </summary>
    public class MultiSubreddit
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public MultiSubredditData? Data { get; set; }
    }

    public class MultiSubredditData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("icon_url")]
        public string? IconUrl { get; set; }

        [JsonPropertyName("key_color")]
        public string? KeyColor { get; set; }

        [JsonPropertyName("primary_color")]
        public string? PrimaryColor { get; set; }

        [JsonPropertyName("community_icon")]
        public string? CommunityIcon { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("subscribers")]
        public int? Subscribers { get; set; }
    }
}
