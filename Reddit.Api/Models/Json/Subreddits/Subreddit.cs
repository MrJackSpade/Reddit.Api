using Reddit.Api.Converters;
using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Subreddits
{
    /// <summary>
    /// Reddit subreddit (t5) data.
    /// </summary>
    public class Subreddit
    {
        [JsonPropertyName("accept_followers")]
        public JsonBool AcceptFollowers { get; set; }

        [JsonPropertyName("accounts_active")]
        public int? AccountsActive { get; set; }

        [JsonPropertyName("active_user_count")]
        public int? ActiveUserCount { get; set; }

        [JsonPropertyName("advertiser_category")]
        public string? AdvertiserCategory { get; set; }

        [JsonPropertyName("allow_chat_post_creation")]
        public JsonBool AllowChatPostCreation { get; set; }

        [JsonPropertyName("allow_galleries")]
        public JsonBool AllowGalleries { get; set; }

        [JsonPropertyName("allow_images")]
        public bool AllowImages { get; set; }

        [JsonPropertyName("allow_polls")]
        public JsonBool AllowPolls { get; set; }

        [JsonPropertyName("allow_predictions")]
        public JsonBool AllowPredictions { get; set; }

        [JsonPropertyName("allow_predictions_tournament")]
        public JsonBool AllowPredictionsTournament { get; set; }

        [JsonPropertyName("allow_talks")]
        public JsonBool AllowTalks { get; set; }

        [JsonPropertyName("allow_videogifs")]
        public bool AllowVideogifs { get; set; }

        [JsonPropertyName("allow_videos")]
        public bool AllowVideos { get; set; }

        [JsonPropertyName("banner_background_color")]
        public JsonColor BannerBackgroundColor { get; set; }

        [JsonPropertyName("banner_background_image")]
        public string? BannerBackgroundImage { get; set; }

        [JsonPropertyName("banner_img")]
        public string? BannerImg { get; set; }

        [JsonPropertyName("banner_size")]
        public List<int>? BannerSize { get; set; }

        [JsonPropertyName("can_assign_link_flair")]
        public JsonBool CanAssignLinkFlair { get; set; }

        [JsonPropertyName("can_assign_user_flair")]
        public JsonBool CanAssignUserFlair { get; set; }

        [JsonPropertyName("collapse_deleted_comments")]
        public JsonBool CollapseDeletedComments { get; set; }

        [JsonPropertyName("comment_score_hide_mins")]
        public int? CommentScoreHideMins { get; set; }

        [JsonPropertyName("community_icon")]
        public string? CommunityIcon { get; set; }

        [JsonPropertyName("created")]
        public JsonDateTime Created { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("description")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? Description { get; set; }

        [JsonPropertyName("description_html")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? DescriptionHtml { get; set; }

        [JsonPropertyName("disable_contributor_requests")]
        public JsonBool DisableContributorRequests { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("display_name_prefixed")]
        public string DisplayNamePrefixed { get; set; } = string.Empty;

        [JsonPropertyName("emojis_custom_size")]
        public List<int>? EmojisCustomSize { get; set; }

        [JsonPropertyName("emojis_enabled")]
        public bool EmojisEnabled { get; set; }

        [JsonPropertyName("free_form_reports")]
        public bool FreeFormReports { get; set; }

        [JsonPropertyName("header_img")]
        public string? HeaderImg { get; set; }

        [JsonPropertyName("header_size")]
        public List<int>? HeaderSize { get; set; }

        [JsonPropertyName("header_title")]
        public string? HeaderTitle { get; set; }

        [JsonPropertyName("hide_ads")]
        public bool HideAds { get; set; }

        [JsonPropertyName("icon_img")]
        public string? IconImg { get; set; }

        [JsonPropertyName("icon_size")]
        public List<int>? IconSize { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("is_chat_post_feature_enabled")]
        public JsonBool IsChatPostFeatureEnabled { get; set; }

        [JsonPropertyName("is_crosspostable_subreddit")]
        public JsonBool IsCrosspostableSubreddit { get; set; }

        [JsonPropertyName("key_color")]
        public JsonColor KeyColor { get; set; }

        [JsonPropertyName("lang")]
        public string? Lang { get; set; }

        [JsonPropertyName("link_flair_enabled")]
        public bool LinkFlairEnabled { get; set; }

        [JsonPropertyName("link_flair_position")]
        public FlairPosition LinkFlairPosition { get; set; }

        [JsonPropertyName("mobile_banner_image")]
        public string? MobileBannerImage { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("notification_level")]
        public string? NotificationLevel { get; set; }

        [JsonPropertyName("original_content_tag_enabled")]
        public bool OriginalContentTagEnabled { get; set; }

        [JsonPropertyName("over18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("primary_color")]
        public JsonColor PrimaryColor { get; set; }

        [JsonPropertyName("public_description")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? PublicDescription { get; set; }

        [JsonPropertyName("public_description_html")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? PublicDescriptionHtml { get; set; }

        [JsonPropertyName("public_traffic")]
        public JsonBool PublicTraffic { get; set; }

        [JsonPropertyName("quarantine")]
        public bool Quarantine { get; set; }

        [JsonPropertyName("restrict_commenting")]
        public bool RestrictCommenting { get; set; }

        [JsonPropertyName("restrict_posting")]
        public bool RestrictPosting { get; set; }

        [JsonPropertyName("show_media")]
        public bool ShowMedia { get; set; }

        [JsonPropertyName("show_media_preview")]
        public bool ShowMediaPreview { get; set; }

        [JsonPropertyName("spoilers_enabled")]
        public bool SpilersEnabled { get; set; }

        [JsonPropertyName("submission_type")]
        public SubmissionType SubmissionType { get; set; }

        [JsonPropertyName("submit_text")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? SubmitText { get; set; }

        [JsonPropertyName("submit_text_html")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? SubmitTextHtml { get; set; }

        [JsonPropertyName("subreddit_type")]
        public SubredditType SubredditType { get; set; }

        [JsonPropertyName("subscribers")]
        public int Subscribers { get; set; }

        [JsonPropertyName("suggested_comment_sort")]
        public CommentSort? SuggestedCommentSort { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("user_flair_background_color")]
        public JsonColor UserFlairBackgroundColor { get; set; }

        [JsonPropertyName("user_flair_css_class")]
        public string? UserFlairCssClass { get; set; }

        [JsonPropertyName("user_flair_enabled_in_sr")]
        public JsonBool UserFlairEnabledInSr { get; set; }

        [JsonPropertyName("user_flair_position")]
        public FlairPosition UserFlairPosition { get; set; }

        [JsonPropertyName("user_flair_richtext")]
        public List<object>? UserFlairRichtext { get; set; }

        [JsonPropertyName("user_flair_template_id")]
        public string? UserFlairTemplateId { get; set; }

        [JsonPropertyName("user_flair_text")]
        public string? UserFlairText { get; set; }

        [JsonPropertyName("user_flair_text_color")]
        public JsonColor UserFlairTextColor { get; set; }

        [JsonPropertyName("user_flair_type")]
        public string? UserFlairType { get; set; }

        [JsonPropertyName("user_has_favorited")]
        public JsonBool UserHasFavorited { get; set; }

        [JsonPropertyName("user_is_banned")]
        public JsonBool UserIsBanned { get; set; }

        [JsonPropertyName("user_is_contributor")]
        public JsonBool UserIsContributor { get; set; }

        [JsonPropertyName("user_is_moderator")]
        public JsonBool UserIsModerator { get; set; }

        [JsonPropertyName("user_is_muted")]
        public JsonBool UserIsMuted { get; set; }

        [JsonPropertyName("user_is_subscriber")]
        public JsonBool UserIsSubscriber { get; set; }

        [JsonPropertyName("whitelist_status")]
        public string? WhitelistStatus { get; set; }

        [JsonPropertyName("wiki_enabled")]
        public JsonBool WikiEnabled { get; set; }

        [JsonPropertyName("wls")]
        public int? Wls { get; set; }
    }
}