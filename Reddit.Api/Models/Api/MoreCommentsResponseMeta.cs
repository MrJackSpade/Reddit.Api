using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class MoreCommentsResponseMeta
    {
        [NotNull]
        [JsonPropertyName("data")]
        public MoreCommentsData Data { get; init; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; init; } = [];
    }
}