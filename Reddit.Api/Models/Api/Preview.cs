using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class Preview
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; init; }

        [JsonPropertyName("images")]
        public List<RemoteImage> Images { get; init; } = [];

        [JsonPropertyName("reddit_video_preview")]
        public string? RedditVideoPreview { get; init; }
    }
}