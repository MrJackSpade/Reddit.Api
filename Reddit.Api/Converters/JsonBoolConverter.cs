using Reddit.Api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Api.Converters
{
    /// <summary>
    /// JSON converter for JsonBool that handles true, false, null, empty string, and other falsy values.
    /// </summary>
    public class JsonBoolConverter : JsonConverter<JsonBool>
    {
        public override JsonBool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return JsonBool.Null;

                case JsonTokenType.True:
                    return JsonBool.True;

                case JsonTokenType.False:
                    return JsonBool.False;

                case JsonTokenType.String:
                    string? stringValue = reader.GetString();
                    if (string.IsNullOrEmpty(stringValue))
                    {
                        return JsonBool.Empty;
                    }
                    // Handle string representations of boolean
                    if (stringValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        return JsonBool.True;
                    }

                    if (stringValue.Equals("false", StringComparison.OrdinalIgnoreCase))
                    {
                        return JsonBool.False;
                    }
                    // Any other string is treated as falsy
                    return JsonBool.False;

                case JsonTokenType.Number:
                    // 0 is false, anything else is true
                    if (reader.TryGetInt64(out long longValue))
                    {
                        return longValue == 0 ? JsonBool.False : JsonBool.True;
                    }

                    if (reader.TryGetDouble(out double doubleValue))
                    {
                        return doubleValue == 0 ? JsonBool.False : JsonBool.True;
                    }

                    return JsonBool.False;

                default:
                    return JsonBool.Null;
            }
        }

        public override void Write(Utf8JsonWriter writer, JsonBool value, JsonSerializerOptions options)
        {
            if (value.IsNull)
            {
                writer.WriteNullValue();
            }
            else if (value.IsEmpty)
            {
                writer.WriteStringValue(string.Empty);
            }
            else
            {
                writer.WriteBooleanValue(value.IsTrue);
            }
        }
    }
}