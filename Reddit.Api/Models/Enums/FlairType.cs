using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Flair type values.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<FlairType>))]
    public enum FlairType
    {
        [JsonStringEnumMemberName("LINK_FLAIR")]
        Link,

        [JsonStringEnumMemberName("USER_FLAIR")]
        User
    }
}