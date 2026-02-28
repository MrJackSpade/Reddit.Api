using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.LiveThreads
{
    /// <summary>
    /// Response from GET /live/{thread}.
    /// </summary>
    public class LiveThreadResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "LiveUpdateEvent";

        [JsonPropertyName("data")]
        public LiveThread? Data { get; set; }
    }

    /// <summary>
    /// Live thread data.
    /// </summary>
    public class LiveThread
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("description_html")]
        public string? DescriptionHtml { get; set; }

        [JsonPropertyName("resources")]
        public string? Resources { get; set; }

        [JsonPropertyName("resources_html")]
        public string? ResourcesHtml { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonPropertyName("websocket_url")]
        public string? WebsocketUrl { get; set; }

        [JsonPropertyName("viewer_count")]
        public int? ViewerCount { get; set; }

        [JsonPropertyName("viewer_count_fuzzed")]
        public bool ViewerCountFuzzed { get; set; }

        [JsonPropertyName("nsfw")]
        public bool Nsfw { get; set; }

        [JsonPropertyName("is_announcement")]
        public bool IsAnnouncement { get; set; }

        [JsonPropertyName("button_cta")]
        public string? ButtonCta { get; set; }

        [JsonPropertyName("original_content_expiry")]
        public double? OriginalContentExpiry { get; set; }

        [JsonPropertyName("original_content_owner_id")]
        public string? OriginalContentOwnerId { get; set; }
    }

    /// <summary>
    /// Live thread update.
    /// </summary>
    public class LiveUpdate
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;

        [JsonPropertyName("body_html")]
        public string? BodyHtml { get; set; }

        [JsonPropertyName("created")]
        public double Created { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonPropertyName("stricken")]
        public bool Stricken { get; set; }

        [JsonPropertyName("embeds")]
        public List<LiveUpdateEmbed>? Embeds { get; set; }

        [JsonPropertyName("mobile_embeds")]
        public List<LiveUpdateEmbed>? MobileEmbeds { get; set; }
    }

    public class LiveUpdateEmbed
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }
    }

    /// <summary>
    /// Request to create a live thread.
    /// </summary>
    public class CreateLiveThreadRequest
    {
        /// <summary>
        /// Live thread title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the live thread.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Resources markdown.
        /// </summary>
        public string? Resources { get; set; }

        /// <summary>
        /// Whether the live thread is NSFW.
        /// </summary>
        public bool? Nsfw { get; set; }
    }
}
