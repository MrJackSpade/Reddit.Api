using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Type of submissions allowed in a subreddit.
    /// </summary>
    [JsonConverter(typeof(EmptyStringEnumConverter<SubmissionType>))]
    public enum SubmissionType
    {
        /// <summary>
        /// Maps to JSON null.
        /// </summary>
        Null,

        /// <summary>
        /// Maps to JSON empty string "".
        /// </summary>
        Empty,

        [JsonStringEnumMemberName("any")]
        Any,

        [JsonStringEnumMemberName("link")]
        Link,

        [JsonStringEnumMemberName("self")]
        Self
    }
}