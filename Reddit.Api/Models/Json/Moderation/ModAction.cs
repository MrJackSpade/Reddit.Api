using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Moderation
{
    /// <summary>
    /// Moderation action from mod log.
    /// </summary>
    public class ModAction
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("mod_id36")]
        public string ModId36 { get; set; } = string.Empty;

        [JsonPropertyName("mod")]
        public string Mod { get; set; } = string.Empty;

        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public string? Details { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("target_fullname")]
        public string? TargetFullname { get; set; }

        [JsonPropertyName("target_author")]
        public string? TargetAuthor { get; set; }

        [JsonPropertyName("target_permalink")]
        public string? TargetPermalink { get; set; }

        [JsonPropertyName("target_body")]
        public string? TargetBody { get; set; }

        [JsonPropertyName("target_title")]
        public string? TargetTitle { get; set; }

        [JsonPropertyName("subreddit")]
        public string Subreddit { get; set; } = string.Empty;

        [JsonPropertyName("subreddit_name_prefixed")]
        public string? SubredditNamePrefixed { get; set; }

        [JsonPropertyName("sr_id36")]
        public string? SrId36 { get; set; }

        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }
    }
}
