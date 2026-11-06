using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Response data from POST /api/morechildren, requested without the .json suffix. In that form
    /// Reddit renders each thing as HTML.
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
    /// A rendered morechildren thing.
    /// </summary>
    public class MoreChildrenThingData
    {
        /// <summary>
        /// The thing rendered as old.reddit.com HTML.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// A comment's rendered body.
        /// </summary>
        [JsonPropertyName("contentHTML")]
        public string ContentHtml { get; set; } = string.Empty;

        /// <summary>
        /// A comment's markdown body.
        /// </summary>
        [JsonPropertyName("contentText")]
        public string ContentText { get; set; } = string.Empty;

        /// <summary>
        /// The thing's fullname, e.g. t1_abc123.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The fullname of the post the thing belongs to.
        /// </summary>
        [JsonPropertyName("link")]
        public string Link { get; set; } = string.Empty;

        /// <summary>
        /// The fullname of the thing's parent.
        /// </summary>
        [JsonPropertyName("parent")]
        public string Parent { get; set; } = string.Empty;
    }
}
