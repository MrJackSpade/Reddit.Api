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
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public object? Data { get; set; }
    }
}
