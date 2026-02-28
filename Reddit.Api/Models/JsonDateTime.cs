using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models
{
    /// <summary>
    /// A DateTime wrapper that distinguishes between null, empty/zero, and valid values.
    /// </summary>
    [JsonConverter(typeof(JsonDateTimeConverter))]
    public readonly struct JsonDateTime : IEquatable<JsonDateTime>
    {
        private readonly DateTime _value;
        private readonly JsonDateTimeState _state;

        private JsonDateTime(DateTime value, JsonDateTimeState state)
        {
            _value = value;
            _state = state;
        }

        /// <summary>
        /// Represents a JSON null value.
        /// </summary>
        public static JsonDateTime Null => new(default, JsonDateTimeState.Null);

        /// <summary>
        /// Represents an empty/zero/false value (e.g., 0, "", false).
        /// </summary>
        public static JsonDateTime Empty => new(default, JsonDateTimeState.Empty);

        /// <summary>
        /// Creates a JsonDateTime with a valid DateTime value.
        /// </summary>
        public static JsonDateTime FromDateTime(DateTime value) => new(value, JsonDateTimeState.HasValue);

        /// <summary>
        /// The underlying DateTime value. Only valid when HasValue is true.
        /// </summary>
        public DateTime Value => _value;

        /// <summary>
        /// Returns true if this represents a valid DateTime (not null or empty).
        /// </summary>
        public bool HasValue => _state == JsonDateTimeState.HasValue;

        /// <summary>
        /// Returns true if this represents a JSON null.
        /// </summary>
        public bool IsNull => _state == JsonDateTimeState.Null;

        /// <summary>
        /// Returns true if this represents an empty/zero/false value.
        /// </summary>
        public bool IsEmpty => _state == JsonDateTimeState.Empty;

        /// <summary>
        /// Returns the Value if HasValue is true, otherwise returns null.
        /// </summary>
        public DateTime? ToNullable() => HasValue ? _value : null;

        public override string ToString()
        {
            return _state switch
            {
                JsonDateTimeState.Null => "null",
                JsonDateTimeState.Empty => "empty",
                JsonDateTimeState.HasValue => _value.ToString("O"),
                _ => "unknown"
            };
        }

        public override bool Equals(object? obj) => obj is JsonDateTime other && Equals(other);

        public bool Equals(JsonDateTime other)
        {
            if (_state != other._state)
            {
                return false;
            }

            return _state != JsonDateTimeState.HasValue || _value == other._value;
        }

        public override int GetHashCode() => HashCode.Combine(_state, _value);

        public static bool operator ==(JsonDateTime left, JsonDateTime right) => left.Equals(right);

        public static bool operator !=(JsonDateTime left, JsonDateTime right) => !left.Equals(right);

        public static implicit operator JsonDateTime(DateTime value) => FromDateTime(value);

        /// <summary>
        /// Implicit conversion to DateTime. Throws if null or empty.
        /// </summary>
        public static implicit operator DateTime(JsonDateTime value)
        {
            if (!value.HasValue)
            {
                throw new InvalidOperationException($"Cannot convert {(value.IsNull ? "null" : "empty")} JsonDateTime to DateTime.");
            }

            return value._value;
        }

        /// <summary>
        /// Implicit conversion to DateTime?. Returns null if null or empty.
        /// </summary>
        public static implicit operator DateTime?(JsonDateTime value) => value.HasValue ? value._value : null;

        private enum JsonDateTimeState : byte
        {
            Null = 0,
            Empty = 1,
            HasValue = 2
        }
    }
}
