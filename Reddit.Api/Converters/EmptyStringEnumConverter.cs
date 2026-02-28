using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Api.Converters
{
    /// <summary>
    /// A JSON converter for enums that handles null and empty strings:
    /// - JSON null -> enum value named "Null"
    /// - JSON "" (empty string) -> enum value named "Empty"
    /// - Other values use JsonStringEnumMemberName attribute
    /// Throws if the expected enum value doesn't exist.
    /// </summary>
    public class EmptyStringEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
    {
        private readonly Dictionary<string, TEnum> _stringToEnum;
        private readonly Dictionary<TEnum, string?> _enumToString;
        private readonly TEnum? _nullValue;
        private readonly TEnum? _emptyValue;

        public EmptyStringEnumConverter()
        {
            _stringToEnum = new Dictionary<string, TEnum>(StringComparer.OrdinalIgnoreCase);
            _enumToString = new Dictionary<TEnum, string?>();

            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var value = (TEnum)field.GetValue(null)!;
                var attr = field.GetCustomAttributes(typeof(JsonStringEnumMemberNameAttribute), false)
                    .OfType<JsonStringEnumMemberNameAttribute>()
                    .FirstOrDefault();

                string? jsonName = attr?.Name;
                string fieldName = field.Name;

                // Track Null value by name
                if (fieldName.Equals("Null", StringComparison.OrdinalIgnoreCase))
                {
                    _nullValue = value;
                    _enumToString[value] = null;
                    continue;
                }

                // Track Empty value by name
                if (fieldName.Equals("Empty", StringComparison.OrdinalIgnoreCase))
                {
                    _emptyValue = value;
                    _enumToString[value] = string.Empty;
                    continue;
                }

                // Use JsonStringEnumMemberName if present, otherwise field name
                string name = jsonName ?? fieldName;
                _stringToEnum[name] = value;
                _enumToString[value] = name;
            }
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                if (_nullValue.HasValue)
                {
                    return _nullValue.Value;
                }

                throw new JsonException($"Enum {typeof(TEnum).Name} does not have a 'Null' value to handle null JSON token");
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string? stringValue = reader.GetString();

                if (stringValue == null)
                {
                    if (_nullValue.HasValue)
                    {
                        return _nullValue.Value;
                    }

                    throw new JsonException($"Enum {typeof(TEnum).Name} does not have a 'Null' value to handle null string");
                }

                if (stringValue.Length == 0)
                {
                    if (_emptyValue.HasValue)
                    {
                        return _emptyValue.Value;
                    }

                    throw new JsonException($"Enum {typeof(TEnum).Name} does not have an 'Empty' value to handle empty string");
                }

                if (_stringToEnum.TryGetValue(stringValue, out var enumValue))
                {
                    return enumValue;
                }

                throw new JsonException($"Unable to convert \"{stringValue}\" to enum {typeof(TEnum).Name}");
            }

            throw new JsonException($"Unexpected token {reader.TokenType} when parsing enum {typeof(TEnum).Name}");
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            if (_enumToString.TryGetValue(value, out var stringValue))
            {
                if (stringValue == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    writer.WriteStringValue(stringValue);
                }
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
