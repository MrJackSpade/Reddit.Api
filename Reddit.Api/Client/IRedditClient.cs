using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Account;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Flair;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Messages;
using Reddit.Api.Models.Json.Moderation;
using Reddit.Api.Models.Json.Multis;
using Reddit.Api.Models.Json.Search;
using Reddit.Api.Models.Json.Subreddits;
using Reddit.Api.Models.Json.Users;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Unified interface for the Reddit API client.
    /// </summary>
    public interface IRedditClient
    {
        #region Authentication

        /// <summary>
        /// The authenticated user's username, or null if not authenticated.
        /// </summary>
        string? AuthenticatedUser { get; }

        /// <summary>
        /// Whether valid credentials are available for authentication.
        /// </summary>
        bool CanAuthenticate { get; }

        /// <summary>
        /// Whether the client is currently authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Authenticate with Reddit using the configured credentials.
        /// </summary>
        Task<bool> AuthenticateAsync(CancellationToken cancellationToken = default);

        #endregion Authentication

        #region Account

        /// <summary>
        /// GET /api/v1/me - Get current user identity.
        /// </summary>
        Task<MeResponse?> GetMeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/v1/me/blocked - Get blocked users.
        /// </summary>
        Task<UserListResponse?> GetMyBlockedAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/v1/me/friends - Get friends list.
        /// </summary>
        Task<UserListResponse?> GetMyFriendsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/v1/me/karma - Get karma breakdown by subreddit.
        /// </summary>
        Task<KarmaResponse?> GetMyKarmaAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/v1/me/prefs - Get user preferences.
        /// </summary>
        Task<PrefsResponse?> GetMyPrefsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/v1/me/trophies - Get user trophies.
        /// </summary>
        Task<TrophyListResponse?> GetMyTrophiesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// PATCH /api/v1/me/prefs - Update user preferences.
        /// </summary>
        Task<PrefsResponse?> UpdateMyPrefsAsync(PrefsResponse prefs, CancellationToken cancellationToken = default);

        #endregion Account

        #region Listings

        /// <summary>
        /// GET /best - Get best posts.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetBestAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /by_id/{names} - Get posts by fullname IDs.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetByIdAsync(IEnumerable<string> fullnames, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /comments/{article} - Get post comments.
        /// </summary>
        Task<(Thing<Link>? Post, Listing<Thing<Comment>>? Comments)> GetCommentsAsync(string articleId, string? commentId = null, string? sort = null, int? limit = null, int? depth = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /controversial or /r/{subreddit}/controversial - Get controversial posts.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetControversialAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /duplicates/{article} - Get duplicate posts.
        /// </summary>
        Task<(Thing<Link>? Post, Listing<Thing<Link>>? Duplicates)> GetDuplicatesAsync(string articleId, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /hot or /r/{subreddit}/hot - Get hot posts.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetHotAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /new or /r/{subreddit}/new - Get new posts.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetNewAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /rising or /r/{subreddit}/rising - Get rising posts.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetRisingAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /top or /r/{subreddit}/top - Get top posts.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetTopAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        #endregion Listings

        #region Links & Comments

        /// <summary>
        /// POST /api/comment - Submit a new comment.
        /// </summary>
        Task<Thing<Comment>?> CommentAsync(string parentFullname, string text, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/del - Delete a link or comment.
        /// </summary>
        Task<bool> DeleteThingAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/editusertext - Edit a self post or comment.
        /// </summary>
        Task<Thing<Comment>?> EditAsync(string fullname, string text, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/follow_post - Follow/unfollow a post.
        /// </summary>
        Task<bool> FollowPostAsync(string fullname, bool follow, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/info - Get info about things by ID or URL.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetInfoAsync(IEnumerable<string>? ids = null, string? url = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/morechildren - Load more comments.
        /// </summary>
        Task<List<Thing<Comment>>?> GetMoreChildrenAsync(string linkFullname, IEnumerable<string> children, string? sort = null, int? depth = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/hide - Hide a link.
        /// </summary>
        Task<bool> HideAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/lock - Lock a thing.
        /// </summary>
        Task<bool> LockAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/marknsfw - Mark as NSFW.
        /// </summary>
        Task<bool> MarkNsfwAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/spoiler - Mark as spoiler.
        /// </summary>
        Task<bool> MarkSpoilerAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/report - Report a thing.
        /// </summary>
        Task<bool> ReportAsync(string fullname, string? reason = null, string? siteReason = null, string? ruleReason = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/save - Save a thing.
        /// </summary>
        Task<bool> SaveAsync(string fullname, string? category = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/set_contest_mode - Set contest mode.
        /// </summary>
        Task<bool> SetContestModeAsync(string fullname, bool state, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/sendreplies - Enable/disable inbox replies.
        /// </summary>
        Task<bool> SetSendRepliesAsync(string fullname, bool state, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/set_subreddit_sticky - Sticky/unsticky a post.
        /// </summary>
        Task<bool> SetStickyAsync(string fullname, bool state, int? num = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/submit - Submit a new post.
        /// </summary>
        Task<SubmitResponseData?> SubmitAsync(SubmitRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/unhide - Unhide a link.
        /// </summary>
        Task<bool> UnhideAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/unlock - Unlock a thing.
        /// </summary>
        Task<bool> UnlockAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/unmarknsfw - Unmark as NSFW.
        /// </summary>
        Task<bool> UnmarkNsfwAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/unspoiler - Unmark as spoiler.
        /// </summary>
        Task<bool> UnmarkSpoilerAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/unsave - Unsave a thing.
        /// </summary>
        Task<bool> UnsaveAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/vote - Vote on a thing.
        /// </summary>
        Task<bool> VoteAsync(string fullname, int direction, CancellationToken cancellationToken = default);

        #endregion Links & Comments

        #region Messages

        /// <summary>
        /// POST /api/compose - Send a private message.
        /// </summary>
        Task<bool> ComposeMessageAsync(string to, string subject, string body, string? fromSr = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/del_msg - Delete a message.
        /// </summary>
        Task<bool> DeleteMessageAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /message/inbox - Get inbox messages.
        /// </summary>
        Task<Listing<Thing<Message>>?> GetInboxAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /message/sent - Get sent messages.
        /// </summary>
        Task<Listing<Thing<Message>>?> GetSentAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /message/unread - Get unread messages.
        /// </summary>
        Task<Listing<Thing<Message>>?> GetUnreadAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/read_all_messages - Mark all messages as read.
        /// </summary>
        Task<bool> ReadAllMessagesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/read_message - Mark message as read.
        /// </summary>
        Task<bool> ReadMessageAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/unread_message - Mark message as unread.
        /// </summary>
        Task<bool> UnreadMessageAsync(string fullname, CancellationToken cancellationToken = default);

        #endregion Messages

        #region Subreddits

        /// <summary>
        /// GET /api/subreddit_autocomplete - Autocomplete subreddit names.
        /// </summary>
        Task<List<SubredditAutocompleteItem>?> AutocompleteSubredditsAsync(string query, bool includeOver18 = false, bool includeProfiles = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /subreddits/mine/moderator - Get moderated subreddits.
        /// </summary>
        Task<Listing<Thing<Subreddit>>?> GetMyModeratedAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /subreddits/mine/subscriber - Get subscribed subreddits.
        /// </summary>
        Task<Listing<Thing<Subreddit>>?> GetMySubscribedAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/v1/{subreddit}/post_requirements - Get post requirements.
        /// </summary>
        Task<PostRequirements?> GetPostRequirementsAsync(string subreddit, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/about - Get subreddit info.
        /// </summary>
        Task<Thing<Subreddit>?> GetSubredditAboutAsync(string subreddit, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/about/rules - Get subreddit rules.
        /// </summary>
        Task<SubredditRulesResponse?> GetSubredditRulesAsync(string subreddit, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /subreddits/search - Search subreddits.
        /// </summary>
        Task<Listing<Thing<Subreddit>>?> SearchSubredditsAsync(string query, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/subscribe - Subscribe/unsubscribe to a subreddit.
        /// </summary>
        Task<bool> SubscribeAsync(string subreddit, bool subscribe, CancellationToken cancellationToken = default);

        #endregion Subreddits

        #region Users

        /// <summary>
        /// POST /api/block_user - Block a user.
        /// </summary>
        Task<bool> BlockUserAsync(string username, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /user/{username}/about - Get user info.
        /// </summary>
        Task<Thing<User>?> GetUserAboutAsync(string username, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /user/{username}/comments - Get user comments.
        /// </summary>
        Task<Listing<Thing<Comment>>?> GetUserCommentsAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/user_data_by_account_ids - Get user data by account IDs.
        /// </summary>
        Task<UserDataByIdsResponse?> GetUserDataByIdsAsync(IEnumerable<string> accountIds, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /user/{username}/downvoted - Get downvoted items (own only).
        /// </summary>
        Task<Listing<Thing<Link>>?> GetUserDownvotedAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /user/{username}/overview - Get user activity.
        /// </summary>
        Task<Listing<Thing<object>>?> GetUserOverviewAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /user/{username}/saved - Get saved items.
        /// </summary>
        Task<Listing<Thing<object>>?> GetUserSavedAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /user/{username}/submitted - Get user posts.
        /// </summary>
        Task<Listing<Thing<Link>>?> GetUserSubmittedAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /user/{username}/upvoted - Get upvoted items (own only).
        /// </summary>
        Task<Listing<Thing<Link>>?> GetUserUpvotedAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        #endregion Users

        #region Moderation

        /// <summary>
        /// POST /api/approve - Approve a thing.
        /// </summary>
        Task<bool> ApproveAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/distinguish - Distinguish a thing.
        /// </summary>
        Task<Thing<Comment>?> DistinguishAsync(string fullname, DistinguishHow how = DistinguishHow.Yes, bool? sticky = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/about/edited - Get edited items.
        /// </summary>
        Task<Listing<Thing<object>>?> GetEditedAsync(string subreddit, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/about/log - Get mod log.
        /// </summary>
        Task<Listing<Thing<ModAction>>?> GetModLogAsync(string subreddit, ListingParameters? parameters = null, string? type = null, string? mod = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/about/modqueue - Get mod queue.
        /// </summary>
        Task<Listing<Thing<object>>?> GetModQueueAsync(string subreddit, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/about/reports - Get reported items.
        /// </summary>
        Task<Listing<Thing<object>>?> GetReportsAsync(string subreddit, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/about/spam - Get spam items.
        /// </summary>
        Task<Listing<Thing<object>>?> GetSpamAsync(string subreddit, ListingParameters? parameters = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/ignore_reports - Ignore reports on a thing.
        /// </summary>
        Task<bool> IgnoreReportsAsync(string fullname, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/remove - Remove a thing.
        /// </summary>
        Task<bool> RemoveAsync(string fullname, bool spam = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/unignore_reports - Unignore reports on a thing.
        /// </summary>
        Task<bool> UnignoreReportsAsync(string fullname, CancellationToken cancellationToken = default);

        #endregion Moderation

        #region Flair

        /// <summary>
        /// POST /r/{subreddit}/api/flairtemplate_v2 - Create/update flair template.
        /// </summary>
        Task<FlairTemplate?> CreateFlairTemplateAsync(string subreddit, FlairTemplateRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/api/flairlist - Get list of users with flair.
        /// </summary>
        Task<FlairListResponse?> GetFlairListAsync(string subreddit, string? after = null, int? limit = null, string? name = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/api/link_flair_v2 - Get link flair templates.
        /// </summary>
        Task<List<FlairTemplate>?> GetLinkFlairAsync(string subreddit, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/api/user_flair_v2 - Get user flair templates.
        /// </summary>
        Task<List<FlairTemplate>?> GetUserFlairAsync(string subreddit, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /r/{subreddit}/api/selectflair - Set flair on a link or user.
        /// </summary>
        Task<bool> SelectFlairAsync(string subreddit, SelectFlairRequest request, CancellationToken cancellationToken = default);

        #endregion Flair

        #region Multis

        /// <summary>
        /// PUT /api/multi/{multipath}/r/{srname} - Add subreddit to multi.
        /// </summary>
        Task<bool> AddSubredditToMultiAsync(string multipath, string subreddit, CancellationToken cancellationToken = default);

        /// <summary>
        /// POST /api/multi/copy - Copy a multi.
        /// </summary>
        Task<MultiResponse?> CopyMultiAsync(string from, string to, string? displayName = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// PUT /api/multi/{multipath} - Create or update a multi.
        /// </summary>
        Task<MultiResponse?> CreateOrUpdateMultiAsync(string multipath, MultiCreateRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// DELETE /api/multi/{multipath} - Delete a multi.
        /// </summary>
        Task<bool> DeleteMultiAsync(string multipath, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/multi/{multipath} - Get multi details.
        /// </summary>
        Task<MultiResponse?> GetMultiAsync(string multipath, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/multi/mine - Get my multireddits.
        /// </summary>
        Task<List<MultiResponse>?> GetMyMultisAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /api/multi/user/{username} - Get user's public multireddits.
        /// </summary>
        Task<List<MultiResponse>?> GetUserMultisAsync(string username, CancellationToken cancellationToken = default);

        /// <summary>
        /// DELETE /api/multi/{multipath}/r/{srname} - Remove subreddit from multi.
        /// </summary>
        Task<bool> RemoveSubredditFromMultiAsync(string multipath, string subreddit, CancellationToken cancellationToken = default);

        #endregion Multis

        #region Search

        /// <summary>
        /// GET /search - Search Reddit.
        /// </summary>
        Task<Listing<Thing<Link>>?> SearchAsync(SearchParameters parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// GET /r/{subreddit}/search - Search within a subreddit.
        /// </summary>
        Task<Listing<Thing<Link>>?> SearchSubredditAsync(string subreddit, SearchParameters parameters, CancellationToken cancellationToken = default);

        #endregion Search
    }
}