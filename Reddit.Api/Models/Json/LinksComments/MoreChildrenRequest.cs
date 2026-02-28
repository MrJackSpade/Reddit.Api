namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Request parameters for GET /api/morechildren.
    /// </summary>
    public class MoreChildrenRequest
    {
        /// <summary>
        /// Comma-delimited list of comment IDs (without t1_ prefix).
        /// </summary>
        public string Children { get; set; } = string.Empty;

        /// <summary>
        /// Maximum depth of subtree (optional).
        /// </summary>
        public int? Depth { get; set; }

        /// <summary>
        /// Fullname of the parent comment (optional for context).
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Whether to return only the immediate children.
        /// </summary>
        public JsonBool LimitChildren { get; set; }

        /// <summary>
        /// Fullname of the link (post).
        /// </summary>
        public string LinkId { get; set; } = string.Empty;

        /// <summary>
        /// Sort order: confidence, top, new, controversial, old, random, qa, live.
        /// </summary>
        public string? Sort { get; set; }
    }
}