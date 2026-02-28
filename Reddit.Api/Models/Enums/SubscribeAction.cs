using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Subscribe action types.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SubscribeAction>))]
    public enum SubscribeAction
    {
        [JsonStringEnumMemberName("sub")]
        Subscribe,

        [JsonStringEnumMemberName("unsub")]
        Unsubscribe
    }
}