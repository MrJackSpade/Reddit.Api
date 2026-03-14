using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Type of content allowed in flair.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<FlairAllowableContent>))]
    public enum FlairAllowableContent
    {
        [JsonStringEnumMemberName("all")]
        All,

        [JsonStringEnumMemberName("emoji")]
        Emoji,

        [JsonStringEnumMemberName("text")]
        Text
    }
}