namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Vote direction values.
    /// </summary>
    public static class VoteDirection
    {
        public const int Downvote = -1;

        public const int None = 0;

        public const int Upvote = 1;
    }

    /// <summary>
    /// Request parameters for POST /api/vote.
    /// </summary>
    public class VoteRequest
    {
        /// <summary>
        /// Vote direction: 1 = upvote, -1 = downvote, 0 = remove vote.
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// Fullname of the thing to vote on.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Ranking id for analytics (optional).
        /// </summary>
        public int? Rank { get; set; }
    }
}