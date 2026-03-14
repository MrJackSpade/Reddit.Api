using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Distinguished status for comments and posts.
    /// </summary>
    [JsonConverter(typeof(EmptyStringEnumConverter<DistinguishedKind>))]
    public enum DistinguishedKind
    {
        /// <summary>
        /// Maps to JSON null.
        /// </summary>
        Null,

        /// <summary>
        /// Maps to JSON empty string "".
        /// </summary>
        Empty,

        [JsonStringEnumMemberName("moderator")]
        Moderator,

        [JsonStringEnumMemberName("admin")]
        Admin
    }
}