namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Request parameters for POST /api/comment.
    /// </summary>
    public class CommentRequest
    {
        /// <summary>
        /// Fullname of the parent thing (t1_ for comment, t3_ for link).
        /// </summary>
        public string ThingId { get; set; } = string.Empty;

        /// <summary>
        /// Raw markdown text of the comment.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// The richtext_json (optional).
        /// </summary>
        public string? RichtextJson { get; set; }

        /// <summary>
        /// Return RTE JSON format.
        /// </summary>
        public bool? ReturnRtjson { get; set; }
    }
}
