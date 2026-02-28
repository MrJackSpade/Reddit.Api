using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Response data from POST /api/morechildren.
    /// </summary>
    public class MoreChildrenResponseData
    {
        [JsonPropertyName("things")]
        public List<MoreChildrenThing> Things { get; set; } = [];
    }

    /// <summary>
    /// Individual thing from morechildren response.
    /// </summary>
    public class MoreChildrenThing
    {
        [JsonPropertyName("data")]
        public object? Data { get; set; }

        [JsonPropertyName("kind")]
        public ThingKind Kind { get; set; }
    }
}