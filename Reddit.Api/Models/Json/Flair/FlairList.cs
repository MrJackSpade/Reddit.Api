using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Flair
{
    /// <summary>
    /// Response from GET /api/flairlist.
    /// </summary>
    public class FlairListResponse
    {
        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("prev")]
        public string? Prev { get; set; }

        [JsonPropertyName("users")]
        public List<UserFlair> Users { get; set; } = [];
    }

    /// <summary>
    /// Individual user flair from flairlist.
    /// </summary>
    public class UserFlair
    {
        [JsonPropertyName("flair_css_class")]
        public string? FlairCssClass { get; set; }

        [JsonPropertyName("flair_text")]
        public string? FlairText { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;
    }
}