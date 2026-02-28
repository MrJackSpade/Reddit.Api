using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Account
{
    /// <summary>
    /// Response from GET /api/v1/me/prefs and request body for PATCH /api/v1/me/prefs.
    /// User preferences.
    /// </summary>
    public class PrefsResponse
    {
        [JsonPropertyName("accept_pms")]
        public string? AcceptPms { get; set; }

        [JsonPropertyName("activity_relevant_ads")]
        public bool? ActivityRelevantAds { get; set; }

        [JsonPropertyName("allow_clicktracking")]
        public bool? AllowClicktracking { get; set; }

        [JsonPropertyName("beta")]
        public bool? Beta { get; set; }

        [JsonPropertyName("clickgadget")]
        public bool? Clickgadget { get; set; }

        [JsonPropertyName("collapse_read_messages")]
        public bool? CollapseReadMessages { get; set; }

        [JsonPropertyName("compress")]
        public bool? Compress { get; set; }

        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("default_comment_sort")]
        public string? DefaultCommentSort { get; set; }

        [JsonPropertyName("domain_details")]
        public bool? DomainDetails { get; set; }

        [JsonPropertyName("email_chat_request")]
        public bool? EmailChatRequest { get; set; }

        [JsonPropertyName("email_comment_reply")]
        public bool? EmailCommentReply { get; set; }

        [JsonPropertyName("email_digests")]
        public bool? EmailDigests { get; set; }

        [JsonPropertyName("email_messages")]
        public bool? EmailMessages { get; set; }

        [JsonPropertyName("email_post_reply")]
        public bool? EmailPostReply { get; set; }

        [JsonPropertyName("email_private_message")]
        public bool? EmailPrivateMessage { get; set; }

        [JsonPropertyName("email_unsubscribe_all")]
        public bool? EmailUnsubscribeAll { get; set; }

        [JsonPropertyName("email_upvote_comment")]
        public bool? EmailUpvoteComment { get; set; }

        [JsonPropertyName("email_upvote_post")]
        public bool? EmailUpvotePost { get; set; }

        [JsonPropertyName("email_user_new_follower")]
        public bool? EmailUserNewFollower { get; set; }

        [JsonPropertyName("email_username_mention")]
        public bool? EmailUsernameMention { get; set; }

        [JsonPropertyName("enable_default_themes")]
        public bool? EnableDefaultThemes { get; set; }

        [JsonPropertyName("enable_followers")]
        public bool? EnableFollowers { get; set; }

        [JsonPropertyName("feed_recommendations_enabled")]
        public bool? FeedRecommendationsEnabled { get; set; }

        [JsonPropertyName("geopopular")]
        public string? Geopopular { get; set; }

        [JsonPropertyName("hide_ads")]
        public bool? HideAds { get; set; }

        [JsonPropertyName("hide_downs")]
        public bool? HideDowns { get; set; }

        [JsonPropertyName("hide_from_robots")]
        public bool? HideFromRobots { get; set; }

        [JsonPropertyName("hide_ups")]
        public bool? HideUps { get; set; }

        [JsonPropertyName("highlight_controversial")]
        public bool? HighlightControversial { get; set; }

        [JsonPropertyName("highlight_new_comments")]
        public bool? HighlightNewComments { get; set; }

        [JsonPropertyName("ignore_suggested_sort")]
        public bool? IgnoreSuggestedSort { get; set; }

        [JsonPropertyName("in_redesign_beta")]
        public bool? InRedesignBeta { get; set; }

        [JsonPropertyName("label_nsfw")]
        public bool? LabelNsfw { get; set; }

        [JsonPropertyName("lang")]
        public string? Lang { get; set; }

        [JsonPropertyName("legacy_search")]
        public bool? LegacySearch { get; set; }

        [JsonPropertyName("live_orangereds")]
        public bool? LiveOrangereds { get; set; }

        [JsonPropertyName("mark_messages_read")]
        public bool? MarkMessagesRead { get; set; }

        [JsonPropertyName("media")]
        public string? Media { get; set; }

        [JsonPropertyName("media_preview")]
        public string? MediaPreview { get; set; }

        [JsonPropertyName("min_comment_score")]
        public int? MinCommentScore { get; set; }

        [JsonPropertyName("min_link_score")]
        public int? MinLinkScore { get; set; }

        [JsonPropertyName("monitor_mentions")]
        public bool? MonitorMentions { get; set; }

        [JsonPropertyName("newwindow")]
        public bool? Newwindow { get; set; }

        [JsonPropertyName("nightmode")]
        public bool? Nightmode { get; set; }

        [JsonPropertyName("no_profanity")]
        public bool? NoProfanity { get; set; }

        [JsonPropertyName("num_comments")]
        public int? NumComments { get; set; }

        [JsonPropertyName("numsites")]
        public int? Numsites { get; set; }

        [JsonPropertyName("organic")]
        public bool? Organic { get; set; }

        [JsonPropertyName("other_theme")]
        public string? OtherTheme { get; set; }

        [JsonPropertyName("over_18")]
        public bool? Over18 { get; set; }

        [JsonPropertyName("private_feeds")]
        public bool? PrivateFeeds { get; set; }

        [JsonPropertyName("profile_opt_out")]
        public bool? ProfileOptOut { get; set; }

        [JsonPropertyName("public_votes")]
        public bool? PublicVotes { get; set; }

        [JsonPropertyName("research")]
        public bool? Research { get; set; }

        [JsonPropertyName("search_include_over_18")]
        public bool? SearchIncludeOver18 { get; set; }

        [JsonPropertyName("send_crosspost_messages")]
        public bool? SendCrosspostMessages { get; set; }

        [JsonPropertyName("send_welcome_messages")]
        public bool? SendWelcomeMessages { get; set; }

        [JsonPropertyName("show_flair")]
        public bool? ShowFlair { get; set; }

        [JsonPropertyName("show_gold_expiration")]
        public bool? ShowGoldExpiration { get; set; }

        [JsonPropertyName("show_link_flair")]
        public bool? ShowLinkFlair { get; set; }

        [JsonPropertyName("show_location_based_recommendations")]
        public bool? ShowLocationBasedRecommendations { get; set; }

        [JsonPropertyName("show_presence")]
        public bool? ShowPresence { get; set; }

        [JsonPropertyName("show_promote")]
        public bool? ShowPromote { get; set; }

        [JsonPropertyName("show_stylesheets")]
        public bool? ShowStylesheets { get; set; }

        [JsonPropertyName("show_trending")]
        public bool? ShowTrending { get; set; }

        [JsonPropertyName("show_twitter")]
        public bool? ShowTwitter { get; set; }

        [JsonPropertyName("store_visits")]
        public bool? StoreVisits { get; set; }

        [JsonPropertyName("survey_last_seen_time")]
        public double? SurveyLastSeenTime { get; set; }

        [JsonPropertyName("theme_selector")]
        public string? ThemeSelector { get; set; }

        [JsonPropertyName("third_party_data_personalized_ads")]
        public bool? ThirdPartyDataPersonalizedAds { get; set; }

        [JsonPropertyName("third_party_personalized_ads")]
        public bool? ThirdPartyPersonalizedAds { get; set; }

        [JsonPropertyName("third_party_site_data_personalized_ads")]
        public bool? ThirdPartySiteDataPersonalizedAds { get; set; }

        [JsonPropertyName("third_party_site_data_personalized_content")]
        public bool? ThirdPartySiteDataPersonalizedContent { get; set; }

        [JsonPropertyName("threaded_messages")]
        public bool? ThreadedMessages { get; set; }

        [JsonPropertyName("threaded_modmail")]
        public bool? ThreadedModmail { get; set; }

        [JsonPropertyName("top_karma_subreddits")]
        public bool? TopKarmaSubreddits { get; set; }

        [JsonPropertyName("use_global_defaults")]
        public bool? UseGlobalDefaults { get; set; }

        [JsonPropertyName("video_autoplay")]
        public bool? VideoAutoplay { get; set; }
    }
}
