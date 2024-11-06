using Reddit.Api.Interfaces;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class ApiMore : ApiThing, IMore
    {
        [JsonPropertyName("children")]
        public List<string> ChildNames { get; init; } = [];

        [JsonPropertyName("count")]
        public int? Count { get; init; }

        [JsonPropertyName("depth")]
        public int? Depth { get; init; }
    }
}