namespace Reddit.Api.Models.Json.Listings
{
    /// <summary>
    /// Sort options for listings.
    /// </summary>
    public enum ListingSort
    {
        Hot,

        New,

        Top,

        Controversial,

        Rising,

        Best
    }

    /// <summary>
    /// Time period for top/controversial sorting.
    /// </summary>
    public enum TimePeriod
    {
        Hour,

        Day,

        Week,

        Month,

        Year,

        All
    }

    /// <summary>
    /// Parameters for listing endpoints (/hot, /new, /top, etc.)
    /// </summary>
    public class ListingParameters
    {
        /// <summary>
        /// Fullname of the thing to use as anchor.
        /// </summary>
        public string? After { get; set; }

        /// <summary>
        /// Fullname of the thing before the listing.
        /// </summary>
        public string? Before { get; set; }

        /// <summary>
        /// Number of items already seen (for pagination).
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Expand subreddits (for search results).
        /// </summary>
        public JsonBool ExpandSubreddits { get; set; }

        /// <summary>
        /// Geographic region filter.
        /// </summary>
        public string? Geo { get; set; }

        /// <summary>
        /// Include categories in response.
        /// </summary>
        public JsonBool IncludeCategories { get; set; }

        /// <summary>
        /// Maximum number of items to return (1-100, default 25).
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Show items even if user has voted on them.
        /// </summary>
        public JsonBool ShowAll { get; set; }

        /// <summary>
        /// Subreddit detail level.
        /// </summary>
        public string? SrDetail { get; set; }

        /// <summary>
        /// Time period for top/controversial sort (hour, day, week, month, year, all).
        /// </summary>
        public string? Time { get; set; }

        /// <summary>
        /// Convert parameters to query string.
        /// </summary>
        public string ToQueryString()
        {
            return new Client.QueryStringBuilder()
                .Add("after", After)
                .Add("before", Before)
                .Add("limit", Limit)
                .Add("count", Count)
                .AddIf(ShowAll.IsTrue, "show", "all")
                .Add("t", Time)
                .Add("g", Geo)
                .Add("expand_srs", ExpandSubreddits)
                .Add("include_categories", IncludeCategories)
                .Add("sr_detail", SrDetail)
                .Build();
        }
    }
}