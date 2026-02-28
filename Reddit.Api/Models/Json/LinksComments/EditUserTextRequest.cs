namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Request parameters for POST /api/editusertext.
    /// </summary>
    public class EditUserTextRequest
    {
        /// <summary>
        /// Fullname of the thing being edited (t1_ or t3_).
        /// </summary>
        public string ThingId { get; set; } = string.Empty;

        /// <summary>
        /// New raw markdown text.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// The richtext_json (optional for RTE).
        /// </summary>
        public string? RichtextJson { get; set; }

        /// <summary>
        /// Return RTE JSON format.
        /// </summary>
        public bool? ReturnRtjson { get; set; }
    }
}
