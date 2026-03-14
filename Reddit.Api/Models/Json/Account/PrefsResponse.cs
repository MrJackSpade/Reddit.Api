using Reddit.Api.Models.Enums;
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
        public AcceptPms? AcceptPms { get; set; }

        [JsonPropertyName("activity_relevant_ads")]
        public JsonBool ActivityRelevantAds { get; set; }

        [JsonPropertyName("allow_clicktracking")]
        public JsonBool AllowClicktracking { get; set; }

        [JsonPropertyName("beta")]
        public JsonBool Beta { get; set; }

        [JsonPropertyName("clickgadget")]
        public JsonBool Clickgadget { get; set; }

        [JsonPropertyName("collapse_read_messages")]
        public JsonBool CollapseReadMessages { get; set; }

        [JsonPropertyName("compress")]
        public JsonBool Compress { get; set; }

        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("default_comment_sort")]
        public string? DefaultCommentSort { get; set; }

        [JsonPropertyName("domain_details")]
        public JsonBool DomainDetails { get; set; }

        [JsonPropertyName("email_chat_request")]
        public JsonBool EmailChatRequest { get; set; }

        [JsonPropertyName("email_comment_reply")]
        public JsonBool EmailCommentReply { get; set; }

        [JsonPropertyName("email_digests")]
        public JsonBool EmailDigests { get; set; }

        [JsonPropertyName("email_messages")]
        public JsonBool EmailMessages { get; set; }

        [JsonPropertyName("email_post_reply")]
        public JsonBool EmailPostReply { get; set; }

        [JsonPropertyName("email_private_message")]
        public JsonBool EmailPrivateMessage { get; set; }

        [JsonPropertyName("email_unsubscribe_all")]
        public JsonBool EmailUnsubscribeAll { get; set; }

        [JsonPropertyName("email_upvote_comment")]
        public JsonBool EmailUpvoteComment { get; set; }

        [JsonPropertyName("email_upvote_post")]
        public JsonBool EmailUpvotePost { get; set; }

        [JsonPropertyName("email_username_mention")]
        public JsonBool EmailUsernameMention { get; set; }

        [JsonPropertyName("email_user_new_follower")]
        public JsonBool EmailUserNewFollower { get; set; }

        [JsonPropertyName("enable_default_themes")]
        public JsonBool EnableDefaultThemes { get; set; }

        [JsonPropertyName("enable_followers")]
        public JsonBool EnableFollowers { get; set; }

        [JsonPropertyName("feed_recommendations_enabled")]
        public JsonBool FeedRecommendationsEnabled { get; set; }

        [JsonPropertyName("geopopular")]
        public string? Geopopular { get; set; }

        [JsonPropertyName("hide_ads")]
        public JsonBool HideAds { get; set; }

        [JsonPropertyName("hide_downs")]
        public JsonBool HideDowns { get; set; }

        [JsonPropertyName("hide_from_robots")]
        public JsonBool HideFromRobots { get; set; }

        [JsonPropertyName("hide_ups")]
        public JsonBool HideUps { get; set; }

        [JsonPropertyName("highlight_controversial")]
        public JsonBool HighlightControversial { get; set; }

        [JsonPropertyName("highlight_new_comments")]
        public JsonBool HighlightNewComments { get; set; }

        [JsonPropertyName("ignore_suggested_sort")]
        public JsonBool IgnoreSuggestedSort { get; set; }

        [JsonPropertyName("in_redesign_beta")]
        public JsonBool InRedesignBeta { get; set; }

        [JsonPropertyName("label_nsfw")]
        public JsonBool LabelNsfw { get; set; }

        [JsonPropertyName("lang")]
        public string? Lang { get; set; }

        [JsonPropertyName("legacy_search")]
        public JsonBool LegacySearch { get; set; }

        [JsonPropertyName("live_orangereds")]
        public JsonBool LiveOrangereds { get; set; }

        [JsonPropertyName("mark_messages_read")]
        public JsonBool MarkMessagesRead { get; set; }

        [JsonPropertyName("media")]
        public string? Media { get; set; }

        [JsonPropertyName("media_preview")]
        public string? MediaPreview { get; set; }

        [JsonPropertyName("min_comment_score")]
        public int? MinCommentScore { get; set; }

        [JsonPropertyName("min_link_score")]
        public int? MinLinkScore { get; set; }

        [JsonPropertyName("monitor_mentions")]
        public JsonBool MonitorMentions { get; set; }

        [JsonPropertyName("newwindow")]
        public JsonBool Newwindow { get; set; }

        [JsonPropertyName("nightmode")]
        public JsonBool Nightmode { get; set; }

        [JsonPropertyName("no_profanity")]
        public JsonBool NoProfanity { get; set; }

        [JsonPropertyName("num_comments")]
        public int? NumComments { get; set; }

        [JsonPropertyName("numsites")]
        public int? Numsites { get; set; }

        [JsonPropertyName("organic")]
        public JsonBool Organic { get; set; }

        [JsonPropertyName("other_theme")]
        public string? OtherTheme { get; set; }

        [JsonPropertyName("over_18")]
        public JsonBool Over18 { get; set; }

        [JsonPropertyName("private_feeds")]
        public JsonBool PrivateFeeds { get; set; }

        [JsonPropertyName("profile_opt_out")]
        public JsonBool ProfileOptOut { get; set; }

        [JsonPropertyName("public_votes")]
        public JsonBool PublicVotes { get; set; }

        [JsonPropertyName("research")]
        public JsonBool Research { get; set; }

        [JsonPropertyName("search_include_over_18")]
        public JsonBool SearchIncludeOver18 { get; set; }

        [JsonPropertyName("send_crosspost_messages")]
        public JsonBool SendCrosspostMessages { get; set; }

        [JsonPropertyName("send_welcome_messages")]
        public JsonBool SendWelcomeMessages { get; set; }

        [JsonPropertyName("show_flair")]
        public JsonBool ShowFlair { get; set; }

        [JsonPropertyName("show_gold_expiration")]
        public JsonBool ShowGoldExpiration { get; set; }

        [JsonPropertyName("show_link_flair")]
        public JsonBool ShowLinkFlair { get; set; }

        [JsonPropertyName("show_location_based_recommendations")]
        public JsonBool ShowLocationBasedRecommendations { get; set; }

        [JsonPropertyName("show_presence")]
        public JsonBool ShowPresence { get; set; }

        [JsonPropertyName("show_promote")]
        public JsonBool ShowPromote { get; set; }

        [JsonPropertyName("show_stylesheets")]
        public JsonBool ShowStylesheets { get; set; }

        [JsonPropertyName("show_trending")]
        public JsonBool ShowTrending { get; set; }

        [JsonPropertyName("show_twitter")]
        public JsonBool ShowTwitter { get; set; }

        [JsonPropertyName("store_visits")]
        public JsonBool StoreVisits { get; set; }

        [JsonPropertyName("survey_last_seen_time")]
        public long? SurveyLastSeenTime { get; set; }

        [JsonPropertyName("theme_selector")]
        public string? ThemeSelector { get; set; }

        [JsonPropertyName("third_party_data_personalized_ads")]
        public JsonBool ThirdPartyDataPersonalizedAds { get; set; }

        [JsonPropertyName("third_party_personalized_ads")]
        public JsonBool ThirdPartyPersonalizedAds { get; set; }

        [JsonPropertyName("third_party_site_data_personalized_ads")]
        public JsonBool ThirdPartySiteDataPersonalizedAds { get; set; }

        [JsonPropertyName("third_party_site_data_personalized_content")]
        public JsonBool ThirdPartySiteDataPersonalizedContent { get; set; }

        [JsonPropertyName("threaded_messages")]
        public JsonBool ThreadedMessages { get; set; }

        [JsonPropertyName("threaded_modmail")]
        public JsonBool ThreadedModmail { get; set; }

        [JsonPropertyName("top_karma_subreddits")]
        public JsonBool TopKarmaSubreddits { get; set; }

        [JsonPropertyName("use_global_defaults")]
        public JsonBool UseGlobalDefaults { get; set; }

        [JsonPropertyName("video_autoplay")]
        public JsonBool VideoAutoplay { get; set; }
    }
}