using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Position of flair display.
    /// </summary>
    [JsonConverter(typeof(EmptyStringEnumConverter<FlairPosition>))]
    public enum FlairPosition
    {
        /// <summary>
        /// Maps to JSON null.
        /// </summary>
        Null,

        /// <summary>
        /// Maps to JSON empty string "".
        /// </summary>
        Empty,

        [JsonStringEnumMemberName("left")]
        Left,

        [JsonStringEnumMemberName("right")]
        Right
    }
}
