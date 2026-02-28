namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Request parameters for POST /api/report.
    /// </summary>
    public class ReportRequest
    {
        /// <summary>
        /// Fullname of the thing to report.
        /// </summary>
        public string ThingId { get; set; } = string.Empty;

        /// <summary>
        /// The reason for the report (max 100 chars).
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Additional information about the report.
        /// </summary>
        public string? OtherReason { get; set; }

        /// <summary>
        /// Site rule name if using sitewide reporting.
        /// </summary>
        public string? SiteReason { get; set; }

        /// <summary>
        /// Rule reason (for subreddit rules).
        /// </summary>
        public string? RuleReason { get; set; }

        /// <summary>
        /// Custom report text.
        /// </summary>
        public string? CustomText { get; set; }

        /// <summary>
        /// Name of item being reported.
        /// </summary>
        public string? FromHelpDesk { get; set; }

        /// <summary>
        /// Whether this is a modmail conversation.
        /// </summary>
        public bool? ModmailConvId { get; set; }

        /// <summary>
        /// Whether to use strict JSON mode.
        /// </summary>
        public bool? StrictJsonParsing { get; set; }

        /// <summary>
        /// Whether to use user-based rules.
        /// </summary>
        public bool? UserBasedHateSpeech { get; set; }
    }
}
