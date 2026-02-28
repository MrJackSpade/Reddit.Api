using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Search sort options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SearchSort>))]
    public enum SearchSort
    {
        [JsonStringEnumMemberName("relevance")]
        Relevance,

        [JsonStringEnumMemberName("hot")]
        Hot,

        [JsonStringEnumMemberName("top")]
        Top,

        [JsonStringEnumMemberName("new")]
        New,

        [JsonStringEnumMemberName("comments")]
        Comments
    }
}
