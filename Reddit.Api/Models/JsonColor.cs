using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models
{
    /// <summary>
    /// A color wrapper that distinguishes between null, empty, and valid color values.
    /// </summary>
    [JsonConverter(typeof(JsonColorConverter))]
    public readonly struct JsonColor : IEquatable<JsonColor>
    {
        private readonly JsonColorState _state;

        private readonly string? _value;

        private JsonColor(string? value, JsonColorState state)
        {
            _value = value;
            _state = state;
        }

        private enum JsonColorState : byte
        {
            Null = 0,

            Empty = 1,

            HasValue = 2
        }

        /// <summary>
        /// Represents an empty string value.
        /// </summary>
        public static JsonColor Empty => new(string.Empty, JsonColorState.Empty);

        /// <summary>
        /// Represents a JSON null value.
        /// </summary>
        public static JsonColor Null => new(null, JsonColorState.Null);

        /// <summary>
        /// Returns true if this represents a valid color (not null or empty).
        /// </summary>
        public bool HasValue => _state == JsonColorState.HasValue;

        /// <summary>
        /// Returns true if this represents an empty string.
        /// </summary>
        public bool IsEmpty => _state == JsonColorState.Empty;

        /// <summary>
        /// Returns true if this represents a JSON null.
        /// </summary>
        public bool IsNull => _state == JsonColorState.Null;

        /// <summary>
        /// The underlying color value. Only valid when HasValue is true.
        /// </summary>
        public string Value => _value ?? string.Empty;

        /// <summary>
        /// Creates a JsonColor with a valid color value.
        /// </summary>
        public static JsonColor FromString(string value) => new(value, JsonColorState.HasValue);

        public static bool operator !=(JsonColor left, JsonColor right) => !left.Equals(right);

        public static bool operator ==(JsonColor left, JsonColor right) => left.Equals(right);

        public override bool Equals(object? obj) => obj is JsonColor other && this.Equals(other);

        public bool Equals(JsonColor other)
        {
            if (_state != other._state)
            {
                return false;
            }

            return _state != JsonColorState.HasValue || _value == other._value;
        }

        public override int GetHashCode() => HashCode.Combine(_state, _value);

        /// <summary>
        /// Returns the color as a hex string, or null if not a valid color.
        /// Use this instead of the null-conditional operator since JsonColor is a struct.
        /// </summary>
        public string? ToHex() => HasValue ? _value : null;

        /// <summary>
        /// Returns the Value if HasValue is true, otherwise returns null.
        /// </summary>
        public string? ToNullableString() => HasValue ? _value : null;

        public override string ToString()
        {
            return _state switch
            {
                JsonColorState.Null => "null",
                JsonColorState.Empty => "empty",
                JsonColorState.HasValue => _value ?? string.Empty,
                _ => "unknown"
            };
        }
    }
}