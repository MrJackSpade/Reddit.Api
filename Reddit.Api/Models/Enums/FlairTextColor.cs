using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    [JsonConverter(typeof(EmptyStringEnumConverter<FlairTextColor>))]
    public enum FlairTextColor
    {
        Null,

        Empty,

        [JsonStringEnumMemberName("dark")]
        Dark,

        [JsonStringEnumMemberName("light")]
        Light
    }
}
