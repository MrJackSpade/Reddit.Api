using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Media
{
    /// <summary>
    /// A richtext document used for comments and posts with inline media.
    /// </summary>
    public class RichTextDocument
    {
        [JsonPropertyName("document")]
        public List<RichTextElement> Document { get; set; } = [];
    }

    /// <summary>
    /// An element within a richtext document.
    /// Element type determines which properties are relevant:
    /// - "par": paragraph, uses Children
    /// - "img": inline image, uses Id
    /// - "text": text run, uses Text and optionally Format
    /// - "link": hyperlink, uses Url and Children
    /// </summary>
    public class RichTextElement
    {
        [JsonPropertyName("e")]
        public RichTextElementType ElementType { get; set; }

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonPropertyName("t")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Text { get; set; }

        [JsonPropertyName("c")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<RichTextElement>? Children { get; set; }

        [JsonPropertyName("u")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Url { get; set; }

        [JsonPropertyName("f")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<List<int>>? Format { get; set; }
    }
}
