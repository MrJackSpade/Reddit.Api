using Reddit.Api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Api.Converters
{
    /// <summary>
    /// Converts JSON string values to JsonColor.
    /// Handles null and empty strings appropriately.
    /// </summary>
    public class JsonColorConverter : JsonConverter<JsonColor>
    {
        public override JsonColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return JsonColor.Null;

                case JsonTokenType.String:
                    string? value = reader.GetString();
                    if (value == null)
                    {
                        return JsonColor.Null;
                    }

                    if (value.Length == 0)
                    {
                        return JsonColor.Empty;
                    }

                    return JsonColor.FromString(value);

                default:
                    throw new JsonException($"Unexpected token type {reader.TokenType} when parsing color");
            }
        }

        public override void Write(Utf8JsonWriter writer, JsonColor value, JsonSerializerOptions options)
        {
            if (value.IsNull)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Value);
            }
        }
    }
}
