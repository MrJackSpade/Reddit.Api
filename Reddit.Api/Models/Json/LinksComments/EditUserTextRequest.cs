namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Request parameters for POST /api/editusertext.
    /// </summary>
    public class EditUserTextRequest
    {
        /// <summary>
        /// Return RTE JSON format.
        /// </summary>
        public JsonBool ReturnRtjson { get; set; }

        /// <summary>
        /// The richtext_json (optional for RTE).
        /// </summary>
        public string? RichtextJson { get; set; }

        /// <summary>
        /// New raw markdown text.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Fullname of the thing being edited (t1_ or t3_).
        /// </summary>
        public string ThingId { get; set; } = string.Empty;
    }
}