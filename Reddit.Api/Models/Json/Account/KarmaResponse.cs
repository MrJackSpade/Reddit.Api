using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Account
{
    /// <summary>
    /// Karma breakdown for a single subreddit.
    /// </summary>
    public class KarmaBreakdown
    {
        [JsonPropertyName("comment_karma")]
        public int CommentKarma { get; set; }

        [JsonPropertyName("link_karma")]
        public int LinkKarma { get; set; }

        [JsonPropertyName("sr")]
        public string Subreddit { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response from GET /api/v1/me/karma - karma breakdown by subreddit.
    /// </summary>
    public class KarmaResponse
    {
        [JsonPropertyName("data")]
        public List<KarmaBreakdown> Data { get; set; } = [];

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "KarmaList";
    }
}