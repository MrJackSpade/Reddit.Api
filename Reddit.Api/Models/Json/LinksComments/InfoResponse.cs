namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Query parameters for GET /api/info.
    /// </summary>
    public class InfoRequest
    {
        /// <summary>
        /// Comma-separated list of fullnames (t1_, t3_, t5_ prefixed IDs).
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// URL to look up (for finding posts by URL).
        /// </summary>
        public string? Url { get; set; }
    }
}