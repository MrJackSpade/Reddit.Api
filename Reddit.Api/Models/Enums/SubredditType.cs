using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Type of subreddit.
    /// </summary>
    [JsonConverter(typeof(EmptyStringEnumConverter<SubredditType>))]
    public enum SubredditType
    {
        /// <summary>
        /// Maps to JSON null.
        /// </summary>
        Null,

        /// <summary>
        /// Maps to JSON empty string "".
        /// </summary>
        Empty,

        [JsonStringEnumMemberName("public")]
        Public,

        [JsonStringEnumMemberName("private")]
        Private,

        [JsonStringEnumMemberName("restricted")]
        Restricted,

        [JsonStringEnumMemberName("archived")]
        Archived,

        [JsonStringEnumMemberName("gold_restricted")]
        GoldRestricted,

        [JsonStringEnumMemberName("employees_only")]
        EmployeesOnly,

        [JsonStringEnumMemberName("user")]
        User
    }
}
