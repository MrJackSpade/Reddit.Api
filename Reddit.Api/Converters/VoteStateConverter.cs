using Reddit.Api.Models.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Api.Converters
{
    /// <summary>
    /// Converts JSON bool? (true/false/null) to VoteState enum.
    /// </summary>
    public class VoteStateConverter : JsonConverter<VoteState>
    {
        public override VoteState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return VoteState.None;
            }

            if (reader.TokenType == JsonTokenType.True)
            {
                return VoteState.Upvote;
            }

            if (reader.TokenType == JsonTokenType.False)
            {
                return VoteState.Downvote;
            }

            throw new JsonException($"Unexpected token type {reader.TokenType} for VoteState");
        }

        public override void Write(Utf8JsonWriter writer, VoteState value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case VoteState.Upvote:
                    writer.WriteBooleanValue(true);
                    break;

                case VoteState.Downvote:
                    writer.WriteBooleanValue(false);
                    break;

                default:
                    writer.WriteNullValue();
                    break;
            }
        }
    }
}