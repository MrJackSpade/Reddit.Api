using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Reddit Thing type identifiers.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ThingKind>))]
    public enum ThingKind
    {
        [JsonStringEnumMemberName("t1")]
        Comment,

        [JsonStringEnumMemberName("t2")]
        Account,

        [JsonStringEnumMemberName("t3")]
        Link,

        [JsonStringEnumMemberName("t4")]
        Message,

        [JsonStringEnumMemberName("t5")]
        Subreddit,

        [JsonStringEnumMemberName("t6")]
        Award,

        [JsonStringEnumMemberName("Listing")]
        Listing,

        [JsonStringEnumMemberName("more")]
        More,

        [JsonStringEnumMemberName("LabeledMulti")]
        Multi,

        [JsonStringEnumMemberName("modaction")]
        ModAction
    }
}