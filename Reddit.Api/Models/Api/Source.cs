﻿using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class Source
    {
        [JsonPropertyName("height")]
        public int Height { get; init; }

        [JsonPropertyName("url")]
        public string? Url { get; init; }

        [JsonPropertyName("width")]
        public int Width { get; init; }
    }
}