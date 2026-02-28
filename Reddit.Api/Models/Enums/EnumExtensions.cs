using System.Reflection;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Extension methods for enums with JsonStringEnumMemberName attributes.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the JSON string value for an enum member.
        /// Returns the JsonStringEnumMemberName value if present, otherwise the enum name in lowercase.
        /// </summary>
        public static string ToJsonString<T>(this T value) where T : struct, Enum
        {
            MemberInfo? memberInfo = typeof(T).GetMember(value.ToString()).FirstOrDefault();
            if (memberInfo != null)
            {
                JsonStringEnumMemberNameAttribute? attribute = memberInfo.GetCustomAttribute<JsonStringEnumMemberNameAttribute>();
                if (attribute != null)
                {
                    return attribute.Name;
                }
            }

            return value.ToString().ToLowerInvariant();
        }
    }
}