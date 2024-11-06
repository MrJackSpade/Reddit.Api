using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class MoreCommentsResponse
    {
        [NotNull]
        [JsonPropertyName("json")]
        public MoreCommentsResponseMeta Json { get; init; }
    }
}