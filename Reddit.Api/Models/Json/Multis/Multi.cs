using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Multis
{
    /// <summary>
    /// Reddit multireddit data.
    /// </summary>
    public class Multi
    {
        [JsonPropertyName("can_edit")]
        public bool CanEdit { get; set; }

        [JsonPropertyName("copied_from")]
        public string? CopiedFrom { get; set; }

        [JsonPropertyName("created")]
        public JsonDateTime Created { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("description_html")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? DescriptionHtml { get; set; }

        [JsonPropertyName("description_md")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? DescriptionMd { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("icon_url")]
        public string? IconUrl { get; set; }

        [JsonPropertyName("is_favorited")]
        public JsonBool IsFavorited { get; set; }

        [JsonPropertyName("is_subscriber")]
        public JsonBool IsSubscriber { get; set; }

        [JsonPropertyName("key_color")]
        public JsonColor KeyColor { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("num_subscribers")]
        public int NumSubscribers { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; } = string.Empty;

        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("subreddits")]
        public List<MultiSubreddit> Subreddits { get; set; } = [];

        [JsonPropertyName("visibility")]
        public string Visibility { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response wrapper for multi operations.
    /// </summary>
    public class MultiResponse
    {
        [JsonPropertyName("data")]
        public Multi? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "LabeledMulti";
    }

    /// <summary>
    /// Subreddit within a multireddit.
    /// </summary>
    public class MultiSubreddit
    {
        [JsonPropertyName("data")]
        public MultiSubredditData? Data { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class MultiSubredditData
    {
        [JsonPropertyName("community_icon")]
        public string? CommunityIcon { get; set; }

        [JsonPropertyName("icon_url")]
        public string? IconUrl { get; set; }

        [JsonPropertyName("key_color")]
        public JsonColor KeyColor { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("primary_color")]
        public JsonColor PrimaryColor { get; set; }

        [JsonPropertyName("subscribers")]
        public int? Subscribers { get; set; }
    }
}