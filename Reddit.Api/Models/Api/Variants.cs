using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class Variants
    {
        [JsonPropertyName("gif")]
        public string? Gif { get; init; }

        [JsonPropertyName("mp4")]
        public string? MP4 { get; init; }

        [JsonPropertyName("nsfw")]
        public RemoteImage? Nsfw { get; init; }

        [JsonPropertyName("obfuscated")]
        public Media? Obfuscated { get; init; }
    }
}