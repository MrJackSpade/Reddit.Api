using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Widgets
{
    /// <summary>
    /// Response from GET /r/{subreddit}/api/widgets.
    /// </summary>
    public class WidgetsResponse
    {
        [JsonPropertyName("items")]
        public Dictionary<string, Widget>? Items { get; set; }

        [JsonPropertyName("layout")]
        public WidgetLayout? Layout { get; set; }
    }

    /// <summary>
    /// Widget layout information.
    /// </summary>
    public class WidgetLayout
    {
        [JsonPropertyName("idCardWidget")]
        public string? IdCardWidget { get; set; }

        [JsonPropertyName("topbar")]
        public WidgetTopbar? Topbar { get; set; }

        [JsonPropertyName("sidebar")]
        public WidgetSidebar? Sidebar { get; set; }
    }

    public class WidgetTopbar
    {
        [JsonPropertyName("order")]
        public List<string>? Order { get; set; }
    }

    public class WidgetSidebar
    {
        [JsonPropertyName("order")]
        public List<string>? Order { get; set; }
    }

    /// <summary>
    /// Base widget data.
    /// </summary>
    public class Widget
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("shortName")]
        public string? ShortName { get; set; }

        [JsonPropertyName("styles")]
        public WidgetStyles? Styles { get; set; }

        // Text widget specific
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("textHtml")]
        public string? TextHtml { get; set; }

        // Button widget specific
        [JsonPropertyName("buttons")]
        public List<WidgetButton>? Buttons { get; set; }

        // Image widget specific
        [JsonPropertyName("data")]
        public List<WidgetImage>? Data { get; set; }

        // Calendar widget specific
        [JsonPropertyName("googleCalendarId")]
        public string? GoogleCalendarId { get; set; }

        // Community list specific
        [JsonPropertyName("communityList")]
        public List<WidgetCommunity>? CommunityList { get; set; }

        // Rules widget specific
        [JsonPropertyName("display")]
        public string? Display { get; set; }
    }

    public class WidgetStyles
    {
        [JsonPropertyName("backgroundColor")]
        public string? BackgroundColor { get; set; }

        [JsonPropertyName("headerColor")]
        public string? HeaderColor { get; set; }
    }

    public class WidgetButton
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("color")]
        public string? Color { get; set; }

        [JsonPropertyName("textColor")]
        public string? TextColor { get; set; }

        [JsonPropertyName("fillColor")]
        public string? FillColor { get; set; }

        [JsonPropertyName("hoverState")]
        public WidgetButtonHover? HoverState { get; set; }
    }

    public class WidgetButtonHover
    {
        [JsonPropertyName("color")]
        public string? Color { get; set; }

        [JsonPropertyName("textColor")]
        public string? TextColor { get; set; }

        [JsonPropertyName("fillColor")]
        public string? FillColor { get; set; }
    }

    public class WidgetImage
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("linkUrl")]
        public string? LinkUrl { get; set; }
    }

    public class WidgetCommunity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("subscribers")]
        public int? Subscribers { get; set; }

        [JsonPropertyName("primaryColor")]
        public string? PrimaryColor { get; set; }

        [JsonPropertyName("iconUrl")]
        public string? IconUrl { get; set; }

        [JsonPropertyName("communityIcon")]
        public string? CommunityIcon { get; set; }

        [JsonPropertyName("isNSFW")]
        public bool IsNsfw { get; set; }
    }
}
