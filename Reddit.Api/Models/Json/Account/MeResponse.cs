using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Account
{
    /// <summary>
    /// Response from GET /api/v1/me - current user identity.
    /// </summary>
    public class MeResponse
    {
        [JsonPropertyName("accept_pms")]
        public AcceptPms? AcceptPms { get; set; }

        [JsonPropertyName("awardee_karma")]
        public int AwardeeKarma { get; set; }

        [JsonPropertyName("awarder_karma")]
        public int AwarderKarma { get; set; }

        [JsonPropertyName("can_create_subreddit")]
        public bool CanCreateSubreddit { get; set; }

        [JsonPropertyName("can_edit_name")]
        public bool CanEditName { get; set; }

        [JsonPropertyName("coins")]
        public int Coins { get; set; }

        [JsonPropertyName("comment_karma")]
        public int CommentKarma { get; set; }

        [JsonPropertyName("created")]
        public JsonDateTime Created { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("features")]
        public Dictionary<string, object>? Features { get; set; }

        [JsonPropertyName("force_password_reset")]
        public bool ForcePasswordReset { get; set; }

        [JsonPropertyName("gold_creddits")]
        public int GoldCreddits { get; set; }

        [JsonPropertyName("gold_expiration")]
        public JsonDateTime GoldExpiration { get; set; }

        [JsonPropertyName("has_android_subscription")]
        public bool HasAndroidSubscription { get; set; }

        [JsonPropertyName("has_ios_subscription")]
        public bool HasIosSubscription { get; set; }

        [JsonPropertyName("has_mail")]
        public bool HasMail { get; set; }

        [JsonPropertyName("has_mod_mail")]
        public bool HasModMail { get; set; }

        [JsonPropertyName("has_paypal_subscription")]
        public bool HasPaypalSubscription { get; set; }

        [JsonPropertyName("has_stripe_subscription")]
        public bool HasStripeSubscription { get; set; }

        [JsonPropertyName("has_subscribed")]
        public bool HasSubscribed { get; set; }

        [JsonPropertyName("has_subscribed_to_premium")]
        public bool HasSubscribedToPremium { get; set; }

        [JsonPropertyName("has_verified_email")]
        public bool HasVerifiedEmail { get; set; }

        [JsonPropertyName("has_visited_new_profile")]
        public bool HasVisitedNewProfile { get; set; }

        [JsonPropertyName("hide_from_robots")]
        public bool HideFromRobots { get; set; }

        [JsonPropertyName("icon_img")]
        public string? IconImg { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("in_beta")]
        public bool InBeta { get; set; }

        [JsonPropertyName("inbox_count")]
        public int InboxCount { get; set; }

        [JsonPropertyName("in_redesign_beta")]
        public bool InRedesignBeta { get; set; }

        [JsonPropertyName("is_employee")]
        public bool IsEmployee { get; set; }

        [JsonPropertyName("is_gold")]
        public bool IsGold { get; set; }

        [JsonPropertyName("is_mod")]
        public bool IsMod { get; set; }

        [JsonPropertyName("is_sponsor")]
        public bool IsSponsor { get; set; }

        [JsonPropertyName("is_suspended")]
        public bool IsSuspended { get; set; }

        [JsonPropertyName("link_karma")]
        public int LinkKarma { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("num_friends")]
        public int NumFriends { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("password_set")]
        public bool PasswordSet { get; set; }

        [JsonPropertyName("pref_autoplay")]
        public bool PrefAutoplay { get; set; }

        [JsonPropertyName("pref_clickgadget")]
        public int PrefClickgadget { get; set; }

        [JsonPropertyName("pref_geopopular")]
        public string? PrefGeopopular { get; set; }

        [JsonPropertyName("pref_nightmode")]
        public bool PrefNightmode { get; set; }

        [JsonPropertyName("pref_no_profanity")]
        public bool PrefNoProfanity { get; set; }

        [JsonPropertyName("pref_show_snoovatar")]
        public bool PrefShowSnoovatar { get; set; }

        [JsonPropertyName("pref_show_trending")]
        public bool PrefShowTrending { get; set; }

        [JsonPropertyName("pref_show_twitter")]
        public bool PrefShowTwitter { get; set; }

        [JsonPropertyName("pref_top_karma_subreddits")]
        public bool PrefTopKarmaSubreddits { get; set; }

        [JsonPropertyName("pref_video_autoplay")]
        public bool PrefVideoAutoplay { get; set; }

        [JsonPropertyName("seen_give_award_tooltip")]
        public bool SeenGiveAwardTooltip { get; set; }

        [JsonPropertyName("seen_layout_switch")]
        public bool SeenLayoutSwitch { get; set; }

        [JsonPropertyName("seen_premium_adblock_modal")]
        public bool SeenPremiumAdblockModal { get; set; }

        [JsonPropertyName("seen_redesign_modal")]
        public bool SeenRedesignModal { get; set; }

        [JsonPropertyName("seen_subreddit_chat_ftux")]
        public bool SeenSubredditChatFtux { get; set; }

        [JsonPropertyName("snoovatar_img")]
        public string? SnoovatarImg { get; set; }

        [JsonPropertyName("suspension_expiration_utc")]
        public JsonDateTime SuspensionExpirationUtc { get; set; }

        [JsonPropertyName("total_karma")]
        public int TotalKarma { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }
}