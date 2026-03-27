using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Media
{
    [JsonConverter(typeof(JsonStringEnumConverter<RichTextElementType>))]
    public enum RichTextElementType
    {
        [JsonStringEnumMemberName("par")]
        Paragraph,

        [JsonStringEnumMemberName("text")]
        Text,

        [JsonStringEnumMemberName("img")]
        Image,

        [JsonStringEnumMemberName("link")]
        Link,

        [JsonStringEnumMemberName("blockquote")]
        Blockquote,

        [JsonStringEnumMemberName("code")]
        Code,

        [JsonStringEnumMemberName("raw")]
        Raw
    }
}
