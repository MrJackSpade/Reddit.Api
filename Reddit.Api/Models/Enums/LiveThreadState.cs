using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// State of a live thread.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<LiveThreadState>))]
    public enum LiveThreadState
    {
        [JsonStringEnumMemberName("live")]
        Live,

        [JsonStringEnumMemberName("complete")]
        Complete
    }
}