using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Setting for who can send private messages.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<AcceptPms>))]
    public enum AcceptPms
    {
        [JsonStringEnumMemberName("everyone")]
        Everyone,

        [JsonStringEnumMemberName("friends")]
        Friends,

        [JsonStringEnumMemberName("disabled")]
        Disabled
    }
}
