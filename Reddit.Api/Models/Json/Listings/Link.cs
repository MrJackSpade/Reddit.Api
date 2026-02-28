using Reddit.Api.Converters;
using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Listings
{
    public class Award
    {
        [JsonPropertyName("award_sub_type")]
        public string? AwardSubType { get; set; }

        [JsonPropertyName("award_type")]
        public string? AwardType { get; set; }

        [JsonPropertyName("coin_price")]
        public int? CoinPrice { get; set; }

        [JsonPropertyName("coin_reward")]
        public int? CoinReward { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("days_of_premium")]
        public int? DaysOfPremium { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("end_date")]
        public double? EndDate { get; set; }

        [JsonPropertyName("icon_height")]
        public int? IconHeight { get; set; }

        [JsonPropertyName("icon_url")]
        public string? IconUrl { get; set; }

        [JsonPropertyName("icon_width")]
        public int? IconWidth { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("is_enabled")]
        public bool IsEnabled { get; set; }

        [JsonPropertyName("is_new")]
        public bool IsNew { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("resized_icons")]
        public List<ImageSource>? ResizedIcons { get; set; }

        [JsonPropertyName("start_date")]
        public double? StartDate { get; set; }

        [JsonPropertyName("subreddit_coin_reward")]
        public int? SubredditCoinReward { get; set; }

        [JsonPropertyName("subreddit_id")]
        public string? SubredditId { get; set; }
    }

    public class FlairRichtext
    {
        [JsonPropertyName("a")]
        public string? EmojiId { get; set; }

        [JsonPropertyName("t")]
        public string? Text { get; set; }

        [JsonPropertyName("e")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("u")]
        public string? Url { get; set; }
    }

    public class GalleryData
    {
        [JsonPropertyName("items")]
        public List<GalleryItem>? Items { get; set; }
    }

    public class GalleryItem
    {
        [JsonPropertyName("caption")]
        public string? Caption { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("media_id")]
        public string MediaId { get; set; } = string.Empty;

        [JsonPropertyName("outbound_url")]
        public string? OutboundUrl { get; set; }
    }

    public class ImageSource
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

    public class ImageVariants
    {
        [JsonPropertyName("gif")]
        public PreviewImage? Gif { get; set; }

        [JsonPropertyName("mp4")]
        public PreviewImage? Mp4 { get; set; }

        [JsonPropertyName("nsfw")]
        public PreviewImage? Nsfw { get; set; }

        [JsonPropertyName("obfuscated")]
        public PreviewImage? Obfuscated { get; set; }
    }

    /// <summary>
    /// Reddit link/post (t3) data.
    /// </summary>
    public class Link
    {
        [JsonPropertyName("all_awardings")]
        public List<Award>? AllAwardings { get; set; }

        [JsonPropertyName("allow_live_comments")]
        public bool AllowLiveComments { get; set; }

        [JsonPropertyName("approved_at_utc")]
        public JsonDateTime ApprovedAtUtc { get; set; }

        [JsonPropertyName("approved_by")]
        public string? ApprovedBy { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

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
        public string? AuthorFlairText { get; set; }

        [JsonPropertyName("author_flair_text_color")]
        public JsonColor AuthorFlairTextColor { get; set; }

        [JsonPropertyName("author_fullname")]
        public string? AuthorFullname { get; set; }

        [JsonPropertyName("banned_at_utc")]
        public JsonDateTime BannedAtUtc { get; set; }

        [JsonPropertyName("banned_by")]
        public string? BannedBy { get; set; }

        [JsonPropertyName("can_gild")]
        public bool CanGild { get; set; }

        [JsonPropertyName("can_mod_post")]
        public bool CanModPost { get; set; }

        [JsonPropertyName("clicked")]
        public bool Clicked { get; set; }

        [JsonPropertyName("content_categories")]
        public List<string>? ContentCategories { get; set; }

        [JsonPropertyName("contest_mode")]
        public bool ContestMode { get; set; }

        [JsonPropertyName("created")]
        public JsonDateTime Created { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("crosspost_parent")]
        public string? CrosspostParent { get; set; }

        [JsonPropertyName("crosspost_parent_list")]
        public List<Link>? CrosspostParentList { get; set; }

        [JsonPropertyName("discussion_type")]
        public string? DiscussionType { get; set; }

        [JsonPropertyName("distinguished")]
        public DistinguishedKind Distinguished { get; set; }

        [JsonPropertyName("domain")]
        public string? Domain { get; set; }

        [JsonPropertyName("downs")]
        public int Downs { get; set; }

        [JsonPropertyName("edited")]
        public JsonDateTime Edited { get; set; }

        [JsonPropertyName("gallery_data")]
        public GalleryData? GalleryData { get; set; }

        [JsonPropertyName("gilded")]
        public int Gilded { get; set; }

        [JsonPropertyName("gildings")]
        public Dictionary<string, int>? Gildings { get; set; }

        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; }

        [JsonPropertyName("hide_score")]
        public bool HideScore { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("is_crosspostable")]
        public bool IsCrosspostable { get; set; }

        [JsonPropertyName("is_gallery")]
        public JsonBool IsGallery { get; set; }

        [JsonPropertyName("is_meta")]
        public bool IsMeta { get; set; }

        [JsonPropertyName("is_original_content")]
        public bool IsOriginalContent { get; set; }

        [JsonPropertyName("is_reddit_media_domain")]
        public bool IsRedditMediaDomain { get; set; }

        [JsonPropertyName("is_robot_indexable")]
        public bool IsRobotIndexable { get; set; }

        [JsonPropertyName("is_self")]
        public bool IsSelf { get; set; }

        [JsonPropertyName("is_video")]
        public bool IsVideo { get; set; }

        [JsonPropertyName("likes")]
        public VoteState Likes { get; set; }

        [JsonPropertyName("link_flair_background_color")]
        public JsonColor LinkFlairBackgroundColor { get; set; }

        [JsonPropertyName("link_flair_css_class")]
        public string? LinkFlairCssClass { get; set; }

        [JsonPropertyName("link_flair_richtext")]
        public List<FlairRichtext>? LinkFlairRichtext { get; set; }

        [JsonPropertyName("link_flair_template_id")]
        public string? LinkFlairTemplateId { get; set; }

        [JsonPropertyName("link_flair_text")]
        public string? LinkFlairText { get; set; }

        [JsonPropertyName("link_flair_text_color")]
        public JsonColor LinkFlairTextColor { get; set; }

        [JsonPropertyName("link_flair_type")]
        public string? LinkFlairType { get; set; }

        [JsonPropertyName("locked")]
        public bool Locked { get; set; }

        [JsonPropertyName("media")]
        public Media? Media { get; set; }

        [JsonPropertyName("media_embed")]
        public MediaEmbed? MediaEmbed { get; set; }

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

        [JsonPropertyName("num_comments")]
        public int NumComments { get; set; }

        [JsonPropertyName("num_crossposts")]
        public int NumCrossposts { get; set; }

        [JsonPropertyName("num_reports")]
        public int? NumReports { get; set; }

        [JsonPropertyName("over_18")]
        public bool Over18 { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; } = string.Empty;

        // null = no vote, true = upvote, false = downvote
        [JsonPropertyName("pinned")]
        public bool Pinned { get; set; }

        [JsonPropertyName("post_hint")]
        public PostHint PostHint { get; set; }

        [JsonPropertyName("preview")]
        public Preview? Preview { get; set; }

        [JsonPropertyName("pwls")]
        public int? Pwls { get; set; }

        [JsonPropertyName("quarantine")]
        public bool Quarantine { get; set; }

        [JsonPropertyName("removal_reason")]
        public string? RemovalReason { get; set; }

        [JsonPropertyName("removed_by")]
        public string? RemovedBy { get; set; }

        [JsonPropertyName("removed_by_category")]
        public string? RemovedByCategory { get; set; }

        [JsonPropertyName("report_reasons")]
        public List<string>? ReportReasons { get; set; }

        [JsonPropertyName("saved")]
        public bool Saved { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("secure_media")]
        public Media? SecureMedia { get; set; }

        [JsonPropertyName("secure_media_embed")]
        public MediaEmbed? SecureMediaEmbed { get; set; }

        [JsonPropertyName("selftext")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? Selftext { get; set; }

        [JsonPropertyName("selftext_html")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? SelftextHtml { get; set; }

        [JsonPropertyName("send_replies")]
        public bool SendReplies { get; set; }

        [JsonPropertyName("spam")]
        public JsonBool Spam { get; set; }

        // Can be bool false or timestamp
        [JsonPropertyName("spoiler")]
        public bool Spoiler { get; set; }

        [JsonPropertyName("stickied")]
        public bool Stickied { get; set; }

        [JsonPropertyName("subreddit")]
        public string Subreddit { get; set; } = string.Empty;

        [JsonPropertyName("subreddit_id")]
        public string? SubredditId { get; set; }

        [JsonPropertyName("subreddit_name_prefixed")]
        public string? SubredditNamePrefixed { get; set; }

        [JsonPropertyName("subreddit_subscribers")]
        public int? SubredditSubscribers { get; set; }

        [JsonPropertyName("subreddit_type")]
        public SubredditType SubredditType { get; set; }

        [JsonPropertyName("suggested_sort")]
        public CommentSort? SuggestedSort { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("thumbnail_height")]
        public int? ThumbnailHeight { get; set; }

        [JsonPropertyName("thumbnail_width")]
        public int? ThumbnailWidth { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("total_awards_received")]
        public int TotalAwardsReceived { get; set; }

        [JsonPropertyName("treatment_tags")]
        public List<string>? TreatmentTags { get; set; }

        [JsonPropertyName("ups")]
        public int Ups { get; set; }

        [JsonPropertyName("upvote_ratio")]
        public double UpvoteRatio { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("url_overridden_by_dest")]
        public string? UrlOverriddenByDest { get; set; }

        [JsonPropertyName("user_reports")]
        public List<List<object>>? UserReports { get; set; }

        [JsonPropertyName("view_count")]
        public int? ViewCount { get; set; }

        [JsonPropertyName("visited")]
        public bool Visited { get; set; }

        [JsonPropertyName("wls")]
        public int? Wls { get; set; }
    }

    public class Media
    {
        [JsonPropertyName("oembed")]
        public OEmbed? Oembed { get; set; }

        [JsonPropertyName("reddit_video")]
        public RedditVideo? RedditVideo { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }

    public class MediaEmbed
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("media_domain_url")]
        public string? MediaDomainUrl { get; set; }

        [JsonPropertyName("scrolling")]
        public JsonBool Scrolling { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }
    }

    public class MediaMetadata
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("m")]
        public string? Mimetype { get; set; }

        [JsonPropertyName("p")]
        public List<ImageSource>? Previews { get; set; }

        [JsonPropertyName("s")]
        public ImageSource? Source { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("e")]
        public string? Type { get; set; }
    }

    public class OEmbed
    {
        [JsonPropertyName("author_name")]
        public string? AuthorName { get; set; }

        [JsonPropertyName("author_url")]
        public string? AuthorUrl { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("html")]
        public string? Html { get; set; }

        [JsonPropertyName("provider_name")]
        public string? ProviderName { get; set; }

        [JsonPropertyName("provider_url")]
        public string? ProviderUrl { get; set; }

        [JsonPropertyName("thumbnail_height")]
        public int? ThumbnailHeight { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string? ThumbnailUrl { get; set; }

        [JsonPropertyName("thumbnail_width")]
        public int? ThumbnailWidth { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }
    }

    public class Preview
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("images")]
        public List<PreviewImage>? Images { get; set; }

        [JsonPropertyName("reddit_video_preview")]
        public RedditVideo? RedditVideoPreview { get; set; }
    }

    public class PreviewImage
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("resolutions")]
        public List<ImageSource>? Resolutions { get; set; }

        [JsonPropertyName("source")]
        public ImageSource? Source { get; set; }

        [JsonPropertyName("variants")]
        public ImageVariants? Variants { get; set; }
    }

    public class RedditVideo
    {
        [JsonPropertyName("dash_url")]
        public string? DashUrl { get; set; }

        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        [JsonPropertyName("fallback_url")]
        public string? FallbackUrl { get; set; }

        [JsonPropertyName("has_audio")]
        public JsonBool HasAudio { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("hls_url")]
        public string? HlsUrl { get; set; }

        [JsonPropertyName("is_gif")]
        public bool IsGif { get; set; }

        [JsonPropertyName("scrubber_media_url")]
        public string? ScrubberMediaUrl { get; set; }

        [JsonPropertyName("transcoding_status")]
        public string? TranscodingStatus { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }
    }
}