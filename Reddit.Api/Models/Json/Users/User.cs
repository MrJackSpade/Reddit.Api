using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Users
{
    /// <summary>
    /// Reddit user account (t2) data.
    /// </summary>
    public class User
    {
        [JsonPropertyName("accept_chats")]
        public bool? AcceptChats { get; set; }

        [JsonPropertyName("accept_followers")]
        public bool? AcceptFollowers { get; set; }

        [JsonPropertyName("accept_pms")]
        public bool? AcceptPms { get; set; }

        [JsonPropertyName("awardee_karma")]
        public int? AwardeeKarma { get; set; }

        [JsonPropertyName("awarder_karma")]
        public int? AwarderKarma { get; set; }

        [JsonPropertyName("comment_karma")]
        public int CommentKarma { get; set; }

        [JsonPropertyName("created")]
        public JsonDateTime Created { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("has_subscribed")]
        public bool? HasSubscribed { get; set; }

        [JsonPropertyName("has_verified_email")]
        public bool? HasVerifiedEmail { get; set; }

        [JsonPropertyName("hide_from_robots")]
        public bool HideFromRobots { get; set; }

        [JsonPropertyName("icon_img")]
        public string? IconImg { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("is_blocked")]
        public bool? IsBlocked { get; set; }

        [JsonPropertyName("is_employee")]
        public bool IsEmployee { get; set; }

        [JsonPropertyName("is_friend")]
        public bool? IsFriend { get; set; }

        [JsonPropertyName("is_gold")]
        public bool IsGold { get; set; }

        [JsonPropertyName("is_mod")]
        public bool IsMod { get; set; }

        [JsonPropertyName("is_suspended")]
        public bool? IsSuspended { get; set; }

        [JsonPropertyName("link_karma")]
        public int LinkKarma { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("pref_show_snoovatar")]
        public bool? PrefShowSnoovatar { get; set; }

        [JsonPropertyName("snoovatar_img")]
        public string? SnoovatarImg { get; set; }

        [JsonPropertyName("snoovatar_size")]
        public List<int>? SnoovatarSize { get; set; }

        [JsonPropertyName("subreddit")]
        public UserSubreddit? Subreddit { get; set; }

        [JsonPropertyName("total_karma")]
        public int? TotalKarma { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }

    /// <summary>
    /// User's profile subreddit information.
    /// </summary>
    public class UserSubreddit
    {
        [JsonPropertyName("accept_followers")]
        public bool? AcceptFollowers { get; set; }

        [JsonPropertyName("banner_img")]
        public string? BannerImg { get; set; }

        [JsonPropertyName("banner_size")]
        public List<int>? BannerSize { get; set; }

        [JsonPropertyName("display_name")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("display_name_prefixed")]
        public string? DisplayNamePrefixed { get; set; }

        [JsonPropertyName("free_form_reports")]
        public bool? FreeFormReports { get; set; }

        [JsonPropertyName("icon_img")]
        public string? IconImg { get; set; }

        [JsonPropertyName("icon_size")]
        public List<int>? IconSize { get; set; }

        [JsonPropertyName("is_default_banner")]
        public bool? IsDefaultBanner { get; set; }

        [JsonPropertyName("is_default_icon")]
        public bool? IsDefaultIcon { get; set; }

        [JsonPropertyName("key_color")]
        public JsonColor KeyColor { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("over_18")]
        public bool? Over18 { get; set; }

        [JsonPropertyName("primary_color")]
        public JsonColor PrimaryColor { get; set; }

        [JsonPropertyName("public_description")]
        public string? PublicDescription { get; set; }

        [JsonPropertyName("subreddit_type")]
        public SubredditType SubredditType { get; set; }

        [JsonPropertyName("subscribers")]
        public int? Subscribers { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("user_is_subscriber")]
        public bool? UserIsSubscriber { get; set; }
    }
}