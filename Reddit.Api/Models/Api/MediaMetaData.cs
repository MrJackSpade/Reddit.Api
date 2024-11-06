﻿using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class MediaMetaData
    {
        [JsonPropertyName("e")]
        public string? E { get; init; }

        [JsonPropertyName("ext")]
        public string? Ext { get; init; }

        [JsonPropertyName("id")]
        public string? Id { get; init; }

        [JsonPropertyName("m")]
        public string? M { get; init; }

        [JsonPropertyName("o")]
        public string? O { get; init; }

        [JsonPropertyName("p")]
        public List<ImageLocation> Previews { get; init; } = [];

        [JsonPropertyName("s")]
        public ImageLocation? Source { get; init; }

        [JsonPropertyName("status")]
        public string? Status { get; init; }

        [JsonPropertyName("t")]
        public string? Text { get; init; }
    }
}