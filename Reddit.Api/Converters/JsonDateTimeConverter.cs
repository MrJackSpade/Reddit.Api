using Reddit.Api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Api.Converters
{
    /// <summary>
    /// Converts Unix timestamps (seconds since epoch) to JsonDateTime.
    /// Handles null, 0, empty string, and boolean false appropriately.
    /// </summary>
    public class JsonDateTimeConverter : JsonConverter<JsonDateTime>
    {
        private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override JsonDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return JsonDateTime.Null;

                case JsonTokenType.False:
                    // Reddit uses "false" for unedited posts/comments
                    return JsonDateTime.Empty;

                case JsonTokenType.True:
                    // Shouldn't happen, but treat as empty
                    return JsonDateTime.Empty;

                case JsonTokenType.Number:
                    double timestamp = reader.GetDouble();
                    if (timestamp <= 0)
                    {
                        return JsonDateTime.Empty;
                    }

                    return JsonDateTime.FromDateTime(UnixEpoch.AddSeconds(timestamp));

                case JsonTokenType.String:
                    string? str = reader.GetString();
                    if (string.IsNullOrEmpty(str))
                    {
                        return JsonDateTime.Empty;
                    }

                    if (double.TryParse(str, out double parsedTimestamp) && parsedTimestamp > 0)
                    {
                        return JsonDateTime.FromDateTime(UnixEpoch.AddSeconds(parsedTimestamp));
                    }

                    return JsonDateTime.Empty;

                default:
                    throw new JsonException($"Unexpected token type {reader.TokenType} when parsing Unix timestamp");
            }
        }

        public override void Write(Utf8JsonWriter writer, JsonDateTime value, JsonSerializerOptions options)
        {
            if (value.IsNull)
            {
                writer.WriteNullValue();
            }
            else if (value.IsEmpty)
            {
                writer.WriteNumberValue(0);
            }
            else
            {
                double timestamp = (value.Value.ToUniversalTime() - UnixEpoch).TotalSeconds;
                writer.WriteNumberValue(timestamp);
            }
        }
    }
}