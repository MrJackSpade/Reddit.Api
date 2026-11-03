using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Response data from GET /api/morechildren, requested without the .json suffix. In that form
    /// Reddit renders each thing as HTML; only the identifying fields are read from it.
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
        public MoreChildrenThingData? Data { get; set; }

        [JsonPropertyName("kind")]
        public ThingKind Kind { get; set; }
    }

    /// <summary>
    /// The identifying fields of a rendered morechildren thing.
    /// </summary>
    public class MoreChildrenThingData
    {
        /// <summary>
        /// The thing's fullname, e.g. t1_abc123.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The fullname of the thing's parent.
        /// </summary>
        [JsonPropertyName("parent")]
        public string Parent { get; set; } = string.Empty;
    }
}
