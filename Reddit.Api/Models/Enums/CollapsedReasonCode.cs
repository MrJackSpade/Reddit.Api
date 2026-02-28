using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Reason codes for collapsed comments.
    /// </summary>
    [JsonConverter(typeof(EmptyStringEnumConverter<CollapsedReasonCode>))]
    public enum CollapsedReasonCode
    {
        /// <summary>
        /// Maps to JSON null.
        /// </summary>
        Null,

        /// <summary>
        /// Maps to JSON empty string "".
        /// </summary>
        Empty,

        [JsonStringEnumMemberName("score-below-threshold")]
        ScoreBelowThreshold,

        [JsonStringEnumMemberName("low_score")]
        LowScore,

        [JsonStringEnumMemberName("deleted")]
        Deleted,

        [JsonStringEnumMemberName("blocked-author")]
        BlockedAuthor
    }
}
