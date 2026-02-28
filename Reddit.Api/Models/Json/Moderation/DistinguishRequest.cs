using Reddit.Api.Models.Enums;

namespace Reddit.Api.Models.Json.Moderation
{
    /// <summary>
    /// Request parameters for POST /api/distinguish.
    /// </summary>
    public class DistinguishRequest
    {
        /// <summary>
        /// How to distinguish: yes, no, admin, special.
        /// </summary>
        public DistinguishHow How { get; set; } = DistinguishHow.Yes;

        /// <summary>
        /// Fullname of the thing to distinguish.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Whether to sticky the comment (top-level comments only).
        /// </summary>
        public JsonBool Sticky { get; set; }
    }
}