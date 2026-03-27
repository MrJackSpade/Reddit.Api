using Reddit.Api.Models.Enums;

namespace Reddit.Api.Models.Json.Search
{
    /// <summary>
    /// Parameters for GET /search.
    /// </summary>
    public class SearchParameters
    {
        /// <summary>
        /// Pagination anchor (after).
        /// </summary>
        public string? After { get; set; }

        /// <summary>
        /// Pagination anchor (before).
        /// </summary>
        public string? Before { get; set; }

        /// <summary>
        /// Include facets in response.
        /// </summary>
        public JsonBool IncludeFacets { get; set; }

        /// <summary>
        /// Include over 18 content.
        /// </summary>
        public JsonBool IncludeOver18 { get; set; }

        /// <summary>
        /// Number of results (1-100).
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Search query string.
        /// </summary>
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// Restrict to safe content.
        /// </summary>
        public JsonBool RestrictSr { get; set; }

        /// <summary>
        /// Sort order: relevance, hot, top, new, comments.
        /// </summary>
        public SearchSort? Sort { get; set; }

        /// <summary>
        /// Restrict search to subreddit (without r/ prefix).
        /// </summary>
        public string? Subreddit { get; set; }

        /// <summary>
        /// Time period: hour, day, week, month, year, all.
        /// </summary>
        public string? Time { get; set; }

        /// <summary>
        /// Search type: sr (subreddits), link, user.
        /// </summary>
        public SearchType? Type { get; set; }

        /// <summary>
        /// Convert parameters to query string.
        /// </summary>
        public string ToQueryString()
        {
            return new Client.QueryStringBuilder()
                .Add("q", Query)
                .Add("sort", Sort?.ToJsonString())
                .Add("t", Time)
                .Add("type", Type?.ToJsonString())
                .Add("after", After)
                .Add("before", Before)
                .Add("limit", Limit)
                .Add("restrict_sr", RestrictSr)
                .Add("include_facets", IncludeFacets)
                .Add("include_over_18", IncludeOver18)
                .Build();
        }
    }
}