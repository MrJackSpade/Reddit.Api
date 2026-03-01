using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models
{
    /// <summary>
    /// A boolean wrapper that distinguishes between true, false, null, and empty values.
    /// </summary>
    [JsonConverter(typeof(JsonBoolConverter))]
    public readonly struct JsonBool : IEquatable<JsonBool>
    {
        private readonly JsonBoolState _state;

        private JsonBool(bool value, JsonBoolState state)
        {
            Value = value;
            _state = state;
        }

        internal enum JsonBoolState : byte
        {
            Null = 0,

            Empty = 1,

            False = 2,

            True = 3
        }

        /// <summary>
        /// Represents an empty string value.
        /// </summary>
        public static JsonBool Empty => new(false, JsonBoolState.Empty);

        /// <summary>
        /// Represents a false value.
        /// </summary>
        public static JsonBool False => new(false, JsonBoolState.False);

        /// <summary>
        /// Represents a JSON null value.
        /// </summary>
        public static JsonBool Null => new(false, JsonBoolState.Null);

        /// <summary>
        /// Represents a true value.
        /// </summary>
        public static JsonBool True => new(true, JsonBoolState.True);

        /// <summary>
        /// Returns true for any value that's not explicitly true (false, null, empty).
        /// </summary>
        public bool Falsy => _state != JsonBoolState.True;

        /// <summary>
        /// Returns true if this represents an empty string.
        /// </summary>
        public bool IsEmpty => _state == JsonBoolState.Empty;

        /// <summary>
        /// Returns true only if the value is explicitly false.
        /// </summary>
        public bool IsFalse => _state == JsonBoolState.False;

        /// <summary>
        /// Returns true if this represents a JSON null.
        /// </summary>
        public bool IsNull => _state == JsonBoolState.Null;

        /// <summary>
        /// Returns true only if the value is explicitly true.
        /// </summary>
        public bool IsTrue => _state == JsonBoolState.True;

        /// <summary>
        /// Returns true only if the value is explicitly true.
        /// Equivalent to IsTrue.
        /// </summary>
        public bool Truthy => _state == JsonBoolState.True;

        /// <summary>
        /// The underlying boolean value. Only meaningful when IsTrue or IsFalse.
        /// </summary>
        public bool Value { get; }

        /// <summary>
        /// Implicit conversion to bool. Returns false for any falsy value (false, null, empty).
        /// </summary>
        public static implicit operator bool(JsonBool value) => !value.Falsy;

        /// <summary>
        /// Implicit conversion from bool.
        /// </summary>
        public static implicit operator JsonBool(bool value) => value ? True : False;

        public static bool operator !=(JsonBool left, JsonBool right) => !left.Equals(right);

        public static bool operator ==(JsonBool left, JsonBool right) => left.Equals(right);

        public override bool Equals(object? obj) => obj is JsonBool other && this.Equals(other);

        public bool Equals(JsonBool other) => _state == other._state;

        public override int GetHashCode() => _state.GetHashCode();

        public override string ToString()
        {
            return _state switch
            {
                JsonBoolState.Null => "null",
                JsonBoolState.Empty => "empty",
                JsonBoolState.True => "true",
                JsonBoolState.False => "false",
                _ => "unknown"
            };
        }
    }
}