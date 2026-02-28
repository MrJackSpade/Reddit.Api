namespace Reddit.Api.Models.Json.Search
{
    /// <summary>
    /// Parameters for GET /search.
    /// </summary>
    public class SearchParameters
    {
        /// <summary>
        /// Search query string.
        /// </summary>
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// Restrict search to subreddit (without r/ prefix).
        /// </summary>
        public string? Subreddit { get; set; }

        /// <summary>
        /// Sort order: relevance, hot, top, new, comments.
        /// </summary>
        public string? Sort { get; set; }

        /// <summary>
        /// Time period: hour, day, week, month, year, all.
        /// </summary>
        public string? Time { get; set; }

        /// <summary>
        /// Search type: sr (subreddits), link, user.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Pagination anchor (after).
        /// </summary>
        public string? After { get; set; }

        /// <summary>
        /// Pagination anchor (before).
        /// </summary>
        public string? Before { get; set; }

        /// <summary>
        /// Number of results (1-100).
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Restrict to safe content.
        /// </summary>
        public bool? RestrictSr { get; set; }

        /// <summary>
        /// Include facets in response.
        /// </summary>
        public bool? IncludeFacets { get; set; }

        /// <summary>
        /// Include over 18 content.
        /// </summary>
        public bool? IncludeOver18 { get; set; }

        /// <summary>
        /// Convert parameters to query string.
        /// </summary>
        public string ToQueryString()
        {
            var parts = new List<string>
            {
                $"q={Uri.EscapeDataString(Query)}"
            };

            if (!string.IsNullOrEmpty(Sort))
                parts.Add($"sort={Uri.EscapeDataString(Sort)}");
            if (!string.IsNullOrEmpty(Time))
                parts.Add($"t={Uri.EscapeDataString(Time)}");
            if (!string.IsNullOrEmpty(Type))
                parts.Add($"type={Uri.EscapeDataString(Type)}");
            if (!string.IsNullOrEmpty(After))
                parts.Add($"after={Uri.EscapeDataString(After)}");
            if (!string.IsNullOrEmpty(Before))
                parts.Add($"before={Uri.EscapeDataString(Before)}");
            if (Limit.HasValue)
                parts.Add($"limit={Limit.Value}");
            if (RestrictSr.HasValue)
                parts.Add($"restrict_sr={RestrictSr.Value.ToString().ToLower()}");
            if (IncludeFacets.HasValue)
                parts.Add($"include_facets={IncludeFacets.Value.ToString().ToLower()}");
            if (IncludeOver18.HasValue)
                parts.Add($"include_over_18={IncludeOver18.Value.ToString().ToLower()}");

            return "?" + string.Join("&", parts);
        }
    }

    /// <summary>
    /// Search sort options.
    /// </summary>
    public static class SearchSort
    {
        public const string Relevance = "relevance";
        public const string Hot = "hot";
        public const string Top = "top";
        public const string New = "new";
        public const string Comments = "comments";
    }

    /// <summary>
    /// Search type options.
    /// </summary>
    public static class SearchType
    {
        public const string Subreddit = "sr";
        public const string Link = "link";
        public const string User = "user";
    }
}
