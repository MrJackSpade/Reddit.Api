using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Distinguish type values.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<DistinguishHow>))]
    public enum DistinguishHow
    {
        [JsonStringEnumMemberName("yes")]
        Yes,

        [JsonStringEnumMemberName("no")]
        No,

        [JsonStringEnumMemberName("admin")]
        Admin,

        [JsonStringEnumMemberName("special")]
        Special
    }
}
