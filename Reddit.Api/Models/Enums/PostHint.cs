using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Post hint indicating the type of content.
    /// </summary>
    [JsonConverter(typeof(EmptyStringEnumConverter<PostHint>))]
    public enum PostHint
    {
        /// <summary>
        /// Maps to JSON null.
        /// </summary>
        Null,

        /// <summary>
        /// Maps to JSON empty string "".
        /// </summary>
        Empty,

        [JsonStringEnumMemberName("image")]
        Image,

        [JsonStringEnumMemberName("link")]
        Link,

        [JsonStringEnumMemberName("video")]
        Video,

        [JsonStringEnumMemberName("self")]
        Self,

        [JsonStringEnumMemberName("rich:video")]
        RichVideo,

        [JsonStringEnumMemberName("hosted:video")]
        HostedVideo,

        [JsonStringEnumMemberName("gallery")]
        Gallery
    }
}