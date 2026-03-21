using Reddit.Api.Converters;
using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Listings
{
    /// <summary>
    /// Reddit comment (t1) data.
    /// </summary>
    public class Comment
    {
        [JsonPropertyName("all_awardings")]
        public List<Award>? AllAwardings { get; set; }

        [JsonPropertyName("approved_at_utc")]
        public JsonDateTime ApprovedAtUtc { get; set; }

        [JsonPropertyName("approved_by")]
        public string? ApprovedBy { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        [JsonPropertyName("associated_award")]
        public object? AssociatedAward { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("author_flair_background_color")]
        public JsonColor AuthorFlairBackgroundColor { get; set; }

        [JsonPropertyName("author_flair_css_class")]
        public string? AuthorFlairCssClass { get; set; }

        [JsonPropertyName("author_flair_richtext")]
        public List<FlairRichtext>? AuthorFlairRichtext { get; set; }

        [JsonPropertyName("author_flair_template_id")]
        public string? AuthorFlairTemplateId { get; set; }

        [JsonPropertyName("author_flair_text")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? AuthorFlairText { get; set; }

        [JsonPropertyName("author_flair_text_color")]
        public FlairTextColor AuthorFlairTextColor { get; set; }

        [JsonPropertyName("author_fullname")]
        public string? AuthorFullname { get; set; }

        [JsonPropertyName("banned_at_utc")]
        public JsonDateTime BannedAtUtc { get; set; }

        [JsonPropertyName("banned_by")]
        public string? BannedBy { get; set; }

        [JsonPropertyName("body")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string Body { get; set; } = string.Empty;

        [JsonPropertyName("body_html")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? BodyHtml { get; set; }

        [JsonPropertyName("can_gild")]
        public bool CanGild { get; set; }

        [JsonPropertyName("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonPropertyName("children")]
        public List<string>? Children { get; set; }

        [JsonPropertyName("collapsed")]
        public bool Collapsed { get; set; }

        [JsonPropertyName("collapsed_because_crowd_control")]
        public JsonBool CollapsedBecauseCrowdControl { get; set; }

        [JsonPropertyName("collapsed_reason")]
        public string? CollapsedReason { get; set; }

        [JsonPropertyName("collapsed_reason_code")]
        public CollapsedReasonCode CollapsedReasonCode { get; set; }

        [JsonPropertyName("controversiality")]
        public int Controversiality { get; set; }

        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [JsonPropertyName("created")]
        public JsonDateTime Created { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("depth")]
        public int Depth { get; set; }

        [JsonPropertyName("distinguished")]
        public DistinguishedKind Distinguished { get; set; }

        [JsonPropertyName("downs")]
        public int Downs { get; set; }

        [JsonPropertyName("edited")]
        public JsonDateTime Edited { get; set; }

        [JsonPropertyName("gilded")]
        public int Gilded { get; set; }

        [JsonPropertyName("gildings")]
        public Dictionary<string, int>? Gildings { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("is_submitter")]
        public bool IsSubmitter { get; set; }

        [JsonPropertyName("likes")]
        public VoteState Likes { get; set; }

        [JsonPropertyName("link_author")]
        public string? LinkAuthor { get; set; }

        [JsonPropertyName("link_id")]
        public string LinkId { get; set; } = string.Empty;

        [JsonPropertyName("link_permalink")]
        public string? LinkPermalink { get; set; }

        [JsonPropertyName("link_title")]
        public string? LinkTitle { get; set; }

        [JsonPropertyName("link_url")]
        public string? LinkUrl { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        [JsonPropertyName("media_metadata")]
        public Dictionary<string, MediaMetadata>? MediaMetadata { get; set; }

        [JsonPropertyName("mod_note")]
        public string? ModNote { get; set; }

        [JsonPropertyName("mod_reason_by")]
        public string? ModReasonBy { get; set; }

        [JsonPropertyName("mod_reason_title")]
        public string? ModReasonTitle { get; set; }

        [JsonPropertyName("mod_reports")]
        public List<List<object>>? ModReports { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("no_follow")]
        public bool NoFollow { get; set; }

        [JsonPropertyName("num_reports")]
        public int? NumReports { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; } = string.Empty;

        [JsonPropertyName("permalink")]
        public string? Permalink { get; set; }

        [JsonPropertyName("removal_reason")]
        public string? RemovalReason { get; set; }

        [JsonPropertyName("removed_by")]
        public string? RemovedBy { get; set; }

        [JsonPropertyName("removed_by_category")]
        public string? RemovedByCategory { get; set; }

        [JsonPropertyName("replies")]
        public object? Replies { get; set; }

        [JsonPropertyName("report_reasons")]
        public List<string>? ReportReasons { get; set; }

        [JsonPropertyName("saved")]
        public bool Saved { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("score_hidden")]
        public bool ScoreHidden { get; set; }

        [JsonPropertyName("send_replies")]
        public bool SendReplies { get; set; }

        [JsonPropertyName("spam")]
        public JsonBool Spam { get; set; }

        // Can be bool false or timestamp
        [JsonPropertyName("stickied")]
        public bool Stickied { get; set; }

        [JsonPropertyName("subreddit")]
        public string Subreddit { get; set; } = string.Empty;

        [JsonPropertyName("subreddit_id")]
        public string? SubredditId { get; set; }

        [JsonPropertyName("subreddit_name_prefixed")]
        public string? SubredditNamePrefixed { get; set; }

        [JsonPropertyName("subreddit_type")]
        public SubredditType SubredditType { get; set; }

        [JsonPropertyName("top_awarded_type")]
        public string? TopAwardedType { get; set; }

        [JsonPropertyName("total_awards_received")]
        public int TotalAwardsReceived { get; set; }

        [JsonPropertyName("treatment_tags")]
        public List<string>? TreatmentTags { get; set; }

        [JsonPropertyName("unrepliable_reason")]
        public string? UnrepliableReason { get; set; }

        [JsonPropertyName("ups")]
        public int Ups { get; set; }

        [JsonPropertyName("user_reports")]
        public List<List<object>>? UserReports { get; set; }

        // Can be empty string or Listing<Thing<Comment>>

        // For "more" type comments

        // For "more" type - child IDs
    }
}