using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Subreddits
{
    /// <summary>
    /// Site rules flow for reporting.
    /// </summary>
    public class SiteRulesFlow
    {
        [JsonPropertyName("nextStepHeader")]
        public string? NextStepHeader { get; set; }

        [JsonPropertyName("nextStepReasons")]
        public List<SiteRulesFlow>? NextStepReasons { get; set; }

        [JsonPropertyName("reasonText")]
        public string? ReasonText { get; set; }

        [JsonPropertyName("reasonTextToShow")]
        public string? ReasonTextToShow { get; set; }
    }

    /// <summary>
    /// Individual subreddit rule.
    /// </summary>
    public class SubredditRule
    {
        [JsonPropertyName("created_utc")]
        public double CreatedUtc { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("description_html")]
        public string? DescriptionHtml { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; } = string.Empty;

        [JsonPropertyName("violation_reason")]
        public string? ViolationReason { get; set; }
    }

    /// <summary>
    /// Response from GET /r/{subreddit}/about/rules.
    /// </summary>
    public class SubredditRulesResponse
    {
        [JsonPropertyName("rules")]
        public List<SubredditRule> Rules { get; set; } = [];

        [JsonPropertyName("site_rules")]
        public List<string> SiteRules { get; set; } = [];

        [JsonPropertyName("site_rules_flow")]
        public List<SiteRulesFlow>? SiteRulesFlow { get; set; }
    }
}