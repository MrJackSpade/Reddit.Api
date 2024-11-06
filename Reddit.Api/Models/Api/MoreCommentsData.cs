using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class MoreCommentsData
    {
        [JsonPropertyName("things")]
        public List<ApiThing> Things { get; init; } = [];
    }
}