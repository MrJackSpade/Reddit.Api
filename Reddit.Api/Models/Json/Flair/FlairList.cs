using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Flair
{
    /// <summary>
    /// Response from GET /api/flairlist.
    /// </summary>
    public class FlairListResponse
    {
        [JsonPropertyName("users")]
        public List<UserFlair> Users { get; set; } = [];

        [JsonPropertyName("next")]
        public string? Next { get; set; }

        [JsonPropertyName("prev")]
        public string? Prev { get; set; }
    }

    /// <summary>
    /// Individual user flair from flairlist.
    /// </summary>
    public class UserFlair
    {
        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;

        [JsonPropertyName("flair_text")]
        public string? FlairText { get; set; }

        [JsonPropertyName("flair_css_class")]
        public string? FlairCssClass { get; set; }
    }
}
