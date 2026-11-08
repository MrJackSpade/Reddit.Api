using System.Globalization;
using System.Text.RegularExpressions;

namespace Reddit.Api.Client
{
    /// <summary>The timestamps www writes in its markup.</summary>
    internal static partial class ShredditTime
    {
        /// <summary>
        /// "2026-11-06T17:12:34.212000+0000": an offset without a colon, which .NET won't parse as is.
        /// Null when the value isn't one.
        /// </summary>
        public static DateTime? Parse(string value)
        {
            Match time = Timestamp().Match(value);

            return time.Success && DateTimeOffset.TryParse($"{time.Groups["time"].Value}{time.Groups["sign"].Value}{time.Groups["hours"].Value}:{time.Groups["minutes"].Value}", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset parsed)
                ? parsed.UtcDateTime
                : null;
        }

        [GeneratedRegex(@"^(?<time>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?)(?<sign>[+-])(?<hours>\d{2})(?<minutes>\d{2})$")]
        private static partial Regex Timestamp();
    }
}
