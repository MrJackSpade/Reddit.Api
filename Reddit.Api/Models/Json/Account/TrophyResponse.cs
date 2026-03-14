using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Account
{
    /// <summary>
    /// Individual trophy data.
    /// </summary>
    public class Trophy
    {
        [JsonPropertyName("award_id")]
        public string? AwardId { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("granted_at")]
        public double? GrantedAt { get; set; }

        [JsonPropertyName("icon_40")]
        public string? Icon40 { get; set; }

        [JsonPropertyName("icon_70")]
        public string? Icon70 { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class TrophyListData
    {
        [JsonPropertyName("trophies")]
        public List<TrophyWrapper> Trophies { get; set; } = [];
    }

    /// <summary>
    /// Response from GET /api/v1/me/trophies - user trophies list.
    /// </summary>
    public class TrophyListResponse
    {
        [JsonPropertyName("data")]
        public TrophyListData? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "TrophyList";
    }

    public class TrophyWrapper
    {
        [JsonPropertyName("data")]
        public Trophy? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "t6";
    }
}