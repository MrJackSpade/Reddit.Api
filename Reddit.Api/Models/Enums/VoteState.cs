using Reddit.Api.Converters;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Represents the user's vote state on a post or comment.
    /// </summary>
    [JsonConverter(typeof(VoteStateConverter))]
    public enum VoteState
    {
        /// <summary>
        /// No vote (JSON null).
        /// </summary>
        None = 0,

        /// <summary>
        /// Upvoted (JSON true).
        /// </summary>
        Upvote = 1,

        /// <summary>
        /// Downvoted (JSON false).
        /// </summary>
        Downvote = -1
    }
}
