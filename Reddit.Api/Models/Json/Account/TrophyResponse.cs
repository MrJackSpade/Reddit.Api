using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Account
{
    /// <summary>
    /// Response from GET /api/v1/me/trophies - user trophies list.
    /// </summary>
    public class TrophyListResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "TrophyList";

        [JsonPropertyName("data")]
        public TrophyListData? Data { get; set; }
    }

    public class TrophyListData
    {
        [JsonPropertyName("trophies")]
        public List<TrophyWrapper> Trophies { get; set; } = [];
    }

    public class TrophyWrapper
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "t6";

        [JsonPropertyName("data")]
        public Trophy? Data { get; set; }
    }

    /// <summary>
    /// Individual trophy data.
    /// </summary>
    public class Trophy
    {
        [JsonPropertyName("icon_70")]
        public string? Icon70 { get; set; }

        [JsonPropertyName("icon_40")]
        public string? Icon40 { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("award_id")]
        public string? AwardId { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("granted_at")]
        public double? GrantedAt { get; set; }
    }
}
