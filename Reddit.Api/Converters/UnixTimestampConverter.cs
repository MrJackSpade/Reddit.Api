using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Api.Converters
{
    /// <summary>
    /// Converts Unix timestamps (seconds since epoch) to DateTime?.
    /// Handles null, 0, and boolean false as null DateTime.
    /// </summary>
    public class UnixTimestampConverter : JsonConverter<DateTime?>
    {
        private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;

                case JsonTokenType.False:
                    // Reddit uses "false" for unedited posts/comments
                    return null;

                case JsonTokenType.True:
                    // Shouldn't happen, but handle gracefully
                    return null;

                case JsonTokenType.Number:
                    double timestamp = reader.GetDouble();
                    if (timestamp <= 0)
                    {
                        return null;
                    }
                    return UnixEpoch.AddSeconds(timestamp);

                case JsonTokenType.String:
                    string? str = reader.GetString();
                    if (string.IsNullOrEmpty(str))
                    {
                        return null;
                    }
                    if (double.TryParse(str, out double parsedTimestamp) && parsedTimestamp > 0)
                    {
                        return UnixEpoch.AddSeconds(parsedTimestamp);
                    }
                    return null;

                default:
                    throw new JsonException($"Unexpected token type {reader.TokenType} when parsing Unix timestamp");
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                double timestamp = (value.Value.ToUniversalTime() - UnixEpoch).TotalSeconds;
                writer.WriteNumberValue(timestamp);
            }
        }
    }
}
