using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Kind of widget.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<WidgetKind>))]
    public enum WidgetKind
    {
        [JsonStringEnumMemberName("button")]
        Button,

        [JsonStringEnumMemberName("community-list")]
        CommunityList,

        [JsonStringEnumMemberName("custom")]
        Custom,

        [JsonStringEnumMemberName("image")]
        Image,

        [JsonStringEnumMemberName("modactions")]
        ModActions,

        [JsonStringEnumMemberName("post-flair")]
        PostFlair,

        [JsonStringEnumMemberName("text")]
        Text,

        [JsonStringEnumMemberName("textarea")]
        TextArea,

        [JsonStringEnumMemberName("calendar")]
        Calendar,

        [JsonStringEnumMemberName("slider")]
        Slider
    }
}