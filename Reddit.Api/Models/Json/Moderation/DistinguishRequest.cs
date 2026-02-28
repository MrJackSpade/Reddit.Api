namespace Reddit.Api.Models.Json.Moderation
{
    /// <summary>
    /// Distinguish type values.
    /// </summary>
    public static class DistinguishHow
    {
        public const string Admin = "admin";

        public const string No = "no";

        public const string Special = "special";

        public const string Yes = "yes";
    }

    /// <summary>
    /// Request parameters for POST /api/distinguish.
    /// </summary>
    public class DistinguishRequest
    {
        /// <summary>
        /// How to distinguish: "yes", "no", "admin", "special".
        /// </summary>
        public string How { get; set; } = "yes";

        /// <summary>
        /// Fullname of the thing to distinguish.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Whether to sticky the comment (top-level comments only).
        /// </summary>
        public bool? Sticky { get; set; }
    }
}