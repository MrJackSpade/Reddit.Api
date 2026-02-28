using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Multis
{
    /// <summary>
    /// Request to copy a multi.
    /// </summary>
    public class MultiCopyRequest
    {
        /// <summary>
        /// Description for the copy.
        /// </summary>
        public string? DescriptionMd { get; set; }

        /// <summary>
        /// Display name for the copy.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Source multipath.
        /// </summary>
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// Destination multipath.
        /// </summary>
        public string To { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request to create or update a multi.
    /// </summary>
    public class MultiCreateRequest
    {
        /// <summary>
        /// Description in markdown.
        /// </summary>
        public string? DescriptionMd { get; set; }

        /// <summary>
        /// Display name for the multi.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Icon name.
        /// </summary>
        public string? IconName { get; set; }

        /// <summary>
        /// Key color (hex).
        /// </summary>
        public string? KeyColor { get; set; }

        /// <summary>
        /// List of subreddits to include.
        /// </summary>
        public List<MultiSubredditInput>? Subreddits { get; set; }

        /// <summary>
        /// Visibility: private, public, hidden.
        /// </summary>
        public string Visibility { get; set; } = "private";

        /// <summary>
        /// Weighting scheme: classic or fresh.
        /// </summary>
        public string? WeightingScheme { get; set; }
    }

    public class MultiDescription
    {
        [JsonPropertyName("body_html")]
        public string? BodyHtml { get; set; }

        [JsonPropertyName("body_md")]
        public string? BodyMd { get; set; }
    }

    /// <summary>
    /// Request to update multi description.
    /// </summary>
    public class MultiDescriptionRequest
    {
        /// <summary>
        /// Multi description in markdown.
        /// </summary>
        public string BodyMd { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response from GET /api/multi/{multipath}/description.
    /// </summary>
    public class MultiDescriptionResponse
    {
        [JsonPropertyName("data")]
        public MultiDescription? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "LabeledMultiDescription";
    }

    public class MultiSubredditInput
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}