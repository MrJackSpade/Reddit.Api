using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Comment sort options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<CommentSort>))]
    public enum CommentSort
    {
        [JsonStringEnumMemberName("confidence")]
        Confidence,

        [JsonStringEnumMemberName("top")]
        Top,

        [JsonStringEnumMemberName("new")]
        New,

        [JsonStringEnumMemberName("controversial")]
        Controversial,

        [JsonStringEnumMemberName("old")]
        Old,

        [JsonStringEnumMemberName("random")]
        Random,

        [JsonStringEnumMemberName("qa")]
        Qa,

        [JsonStringEnumMemberName("live")]
        Live,

        /// <summary>
        /// Reddit's value for a subreddit's suggested_comment_sort when the subreddit has none.
        /// </summary>
        [JsonStringEnumMemberName("blank")]
        Blank
    }
}