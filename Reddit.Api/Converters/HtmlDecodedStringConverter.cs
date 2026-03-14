using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Reddit.Api.Converters
{
    /// <summary>
    /// JSON converter that automatically HTML decodes string values during deserialization.
    /// </summary>
    public class HtmlDecodedStringConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                return value is null ? null : HttpUtility.HtmlDecode(value);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value);
            }
        }
    }
}