using Reddit.Api.Models.Json.Listings;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Flair
{
    /// <summary>
    /// Flair template from /api/link_flair_v2 or /api/user_flair_v2.
    /// </summary>
    public class FlairTemplate
    {
        [JsonPropertyName("allowable_content")]
        public string? AllowableContent { get; set; }

        [JsonPropertyName("background_color")]
        public string BackgroundColor { get; set; } = string.Empty;

        [JsonPropertyName("css_class")]
        public string? CssClass { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("max_emojis")]
        public int? MaxEmojis { get; set; }

        [JsonPropertyName("mod_only")]
        public bool ModOnly { get; set; }

        [JsonPropertyName("override_css")]
        public bool? OverrideCss { get; set; }

        [JsonPropertyName("richtext")]
        public List<FlairRichtext>? Richtext { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("text_color")]
        public string TextColor { get; set; } = string.Empty;

        [JsonPropertyName("text_editable")]
        public bool TextEditable { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}