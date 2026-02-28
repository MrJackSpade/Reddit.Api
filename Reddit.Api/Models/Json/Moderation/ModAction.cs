using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Moderation
{
    /// <summary>
    /// Moderation action from mod log.
    /// </summary>
    public class ModAction
    {
        [JsonPropertyName("action")]
        public ModActionType Action { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("details")]
        public string? Details { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("mod")]
        public string Mod { get; set; } = string.Empty;

        [JsonPropertyName("mod_id36")]
        public string ModId36 { get; set; } = string.Empty;

        [JsonPropertyName("sr_id36")]
        public string? SrId36 { get; set; }

        [JsonPropertyName("subreddit")]
        public string Subreddit { get; set; } = string.Empty;

        [JsonPropertyName("subreddit_name_prefixed")]
        public string? SubredditNamePrefixed { get; set; }

        [JsonPropertyName("target_author")]
        public string? TargetAuthor { get; set; }

        [JsonPropertyName("target_body")]
        public string? TargetBody { get; set; }

        [JsonPropertyName("target_fullname")]
        public string? TargetFullname { get; set; }

        [JsonPropertyName("target_permalink")]
        public string? TargetPermalink { get; set; }

        [JsonPropertyName("target_title")]
        public string? TargetTitle { get; set; }
    }
}