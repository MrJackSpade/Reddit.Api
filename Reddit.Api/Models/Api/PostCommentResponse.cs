﻿using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class PostCommentData
    {
        [JsonPropertyName("things")]
        public List<ApiThing> Things { get; init; } = [];
    }

    public class PostCommentResponse
    {
        [JsonPropertyName("json")]
        public PostCommentResponseMeta Json { get; init; }
    }

    public class PostCommentResponseMeta
    {
        [JsonPropertyName("data")]
        public PostCommentData Data { get; init; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; init; } = [];
    }
}