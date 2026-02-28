using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Subreddits
{
    /// <summary>
    /// Response from GET /api/v1/{subreddit}/post_requirements.
    /// </summary>
    public class PostRequirements
    {
        [JsonPropertyName("domain_blacklist")]
        public List<string>? DomainBlacklist { get; set; }

        [JsonPropertyName("domain_whitelist")]
        public List<string>? DomainWhitelist { get; set; }

        [JsonPropertyName("body_blacklisted_strings")]
        public List<string>? BodyBlacklistedStrings { get; set; }

        [JsonPropertyName("body_required_strings")]
        public List<string>? BodyRequiredStrings { get; set; }

        [JsonPropertyName("title_blacklisted_strings")]
        public List<string>? TitleBlacklistedStrings { get; set; }

        [JsonPropertyName("title_required_strings")]
        public List<string>? TitleRequiredStrings { get; set; }

        [JsonPropertyName("body_restriction_policy")]
        public string? BodyRestrictionPolicy { get; set; }

        [JsonPropertyName("body_text_max_length")]
        public int? BodyTextMaxLength { get; set; }

        [JsonPropertyName("body_text_min_length")]
        public int? BodyTextMinLength { get; set; }

        [JsonPropertyName("gallery_captions_requirement")]
        public string? GalleryCaptionsRequirement { get; set; }

        [JsonPropertyName("gallery_max_items")]
        public int? GalleryMaxItems { get; set; }

        [JsonPropertyName("gallery_min_items")]
        public int? GalleryMinItems { get; set; }

        [JsonPropertyName("gallery_urls_requirement")]
        public string? GalleryUrlsRequirement { get; set; }

        [JsonPropertyName("guidelines_display_policy")]
        public string? GuidelinesDisplayPolicy { get; set; }

        [JsonPropertyName("guidelines_text")]
        public string? GuidelinesText { get; set; }

        [JsonPropertyName("is_flair_required")]
        public bool IsFlairRequired { get; set; }

        [JsonPropertyName("link_repost_age")]
        public int? LinkRepostAge { get; set; }

        [JsonPropertyName("link_restriction_policy")]
        public string? LinkRestrictionPolicy { get; set; }

        [JsonPropertyName("title_regexes")]
        public List<string>? TitleRegexes { get; set; }

        [JsonPropertyName("title_text_max_length")]
        public int? TitleTextMaxLength { get; set; }

        [JsonPropertyName("title_text_min_length")]
        public int? TitleTextMinLength { get; set; }
    }
}
