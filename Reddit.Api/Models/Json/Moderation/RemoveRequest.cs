namespace Reddit.Api.Models.Json.Moderation
{
    /// <summary>
    /// Request parameters for POST /api/remove.
    /// </summary>
    public class RemoveRequest
    {
        /// <summary>
        /// Fullname of the thing to remove.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Whether this is spam (true) or regular removal (false).
        /// </summary>
        public bool Spam { get; set; }
    }

    /// <summary>
    /// Request parameters for POST /api/approve.
    /// </summary>
    public class ApproveRequest
    {
        /// <summary>
        /// Fullname of the thing to approve.
        /// </summary>
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request parameters for POST /api/ignore_reports.
    /// </summary>
    public class IgnoreReportsRequest
    {
        /// <summary>
        /// Fullname of the thing to ignore reports for.
        /// </summary>
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request parameters for POST /api/unignore_reports.
    /// </summary>
    public class UnignoreReportsRequest
    {
        /// <summary>
        /// Fullname of the thing to unignore reports for.
        /// </summary>
        public string Id { get; set; } = string.Empty;
    }
}
