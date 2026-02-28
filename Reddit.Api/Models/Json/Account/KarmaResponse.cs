using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Account
{
    /// <summary>
    /// Response from GET /api/v1/me/karma - karma breakdown by subreddit.
    /// </summary>
    public class KarmaResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "KarmaList";

        [JsonPropertyName("data")]
        public List<KarmaBreakdown> Data { get; set; } = [];
    }

    /// <summary>
    /// Karma breakdown for a single subreddit.
    /// </summary>
    public class KarmaBreakdown
    {
        [JsonPropertyName("sr")]
        public string Subreddit { get; set; } = string.Empty;

        [JsonPropertyName("link_karma")]
        public int LinkKarma { get; set; }

        [JsonPropertyName("comment_karma")]
        public int CommentKarma { get; set; }
    }
}
