namespace Reddit.Api.Models.Json.Moderation
{
    /// <summary>
    /// Request parameters for POST /api/distinguish.
    /// </summary>
    public class DistinguishRequest
    {
        /// <summary>
        /// Fullname of the thing to distinguish.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// How to distinguish: "yes", "no", "admin", "special".
        /// </summary>
        public string How { get; set; } = "yes";

        /// <summary>
        /// Whether to sticky the comment (top-level comments only).
        /// </summary>
        public bool? Sticky { get; set; }
    }

    /// <summary>
    /// Distinguish type values.
    /// </summary>
    public static class DistinguishHow
    {
        public const string Yes = "yes";
        public const string No = "no";
        public const string Admin = "admin";
        public const string Special = "special";
    }
}
