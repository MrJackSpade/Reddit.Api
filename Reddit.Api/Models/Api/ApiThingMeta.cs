﻿using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<List<Root>>(myJsonResponse);

    public class ApiThingMeta
    {
        [NotNull]
        [JsonPropertyName("data")]
        public ApiThing Data { get; init; }

        [JsonPropertyName("kind")]
        public ThingKind Kind { get; init; }
    }
}