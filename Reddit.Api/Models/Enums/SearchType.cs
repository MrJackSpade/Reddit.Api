using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Search type options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SearchType>))]
    public enum SearchType
    {
        [JsonStringEnumMemberName("link")]
        Link,

        [JsonStringEnumMemberName("sr")]
        Subreddit,

        [JsonStringEnumMemberName("user")]
        User
    }
}