using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Types of Reddit submissions.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SubmitKind>))]
    public enum SubmitKind
    {
        [JsonStringEnumMemberName("self")]
        Self,

        [JsonStringEnumMemberName("link")]
        Link,

        [JsonStringEnumMemberName("image")]
        Image,

        [JsonStringEnumMemberName("video")]
        Video,

        [JsonStringEnumMemberName("videogif")]
        VideoGif,

        [JsonStringEnumMemberName("crosspost")]
        Crosspost,

        [JsonStringEnumMemberName("poll")]
        Poll
    }
}
