﻿using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class ApiThingCollectionMeta
    {
        [NotNull]
        [JsonPropertyName("data")]
        public ApiThingCollection Data { get; init; }

        [JsonPropertyName("kind")]
        public ThingKind Kind { get; init; }
    }
}