using Reddit.Api.Client;
using Reddit.Api.Models;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Account;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Emoji;
using Reddit.Api.Models.Json.Flair;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.LiveThreads;
using Reddit.Api.Models.Json.Media;
using Reddit.Api.Models.Json.Messages;
using Reddit.Api.Models.Json.Moderation;
using Reddit.Api.Models.Json.Modmail;
using Reddit.Api.Models.Json.Multis;
using Reddit.Api.Models.Json.Search;
using Reddit.Api.Models.Json.Subreddits;
using Reddit.Api.Models.Json.Users;
using Reddit.Api.Models.Json.Widgets;
using Reddit.Api.Models.Json.Wiki;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Reddit.Api.Json
{
    /// <summary>
    /// Reddit.Api's JSON contract, generated at compile time. Only used where code can't be
    /// generated at run time (NativeAOT, e.g. the Android build): there, reflection-based JSON
    /// can't build generic types such as <c>List&lt;Thing&lt;object&gt;&gt;</c> that nothing
    /// referenced statically. Everywhere else, the reflection path stays exactly as it was.
    /// Lists every type the client reads or writes; types they contain are included with them.
    /// </summary>
    [JsonSourceGenerationOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true)]
    // Account
    [JsonSerializable(typeof(MeResponse))]
    [JsonSerializable(typeof(UserListResponse))]
    [JsonSerializable(typeof(KarmaResponse))]
    [JsonSerializable(typeof(PrefsResponse))]
    [JsonSerializable(typeof(TrophyListResponse))]
    [JsonSerializable(typeof(OAuthToken))]
    [JsonSerializable(typeof(BearerToken))]
    // Listings and things
    [JsonSerializable(typeof(Listing<Thing<object>>))]
    [JsonSerializable(typeof(Listing<Thing<Link>>))]
    [JsonSerializable(typeof(Listing<Thing<Comment>>))]
    [JsonSerializable(typeof(Listing<Thing<Message>>))]
    [JsonSerializable(typeof(Listing<Thing<Subreddit>>))]
    [JsonSerializable(typeof(Listing<Thing<ModAction>>))]
    [JsonSerializable(typeof(Listing<Thing<LiveUpdate>>))]
    [JsonSerializable(typeof(Listing<Thing<WikiRevision>>))]
    [JsonSerializable(typeof(Thing<Link>))]
    [JsonSerializable(typeof(Thing<Comment>))]
    [JsonSerializable(typeof(Thing<Subreddit>))]
    [JsonSerializable(typeof(Thing<User>))]
    [JsonSerializable(typeof(List<Thing<Comment>>))]
    [JsonSerializable(typeof(Link))]
    [JsonSerializable(typeof(Comment))]
    [JsonSerializable(typeof(Message))]
    [JsonSerializable(typeof(ListingParameters))]
    [JsonSerializable(typeof(SearchParameters))]
    [JsonSerializable(typeof(JsonElement))]
    [JsonSerializable(typeof(object))]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(string))]
    // Writes
    [JsonSerializable(typeof(ApiResponse<object>))]
    [JsonSerializable(typeof(ApiResponse<CommentResponseData>))]
    [JsonSerializable(typeof(ApiResponse<SubmitResponseData>))]
    [JsonSerializable(typeof(ApiResponse<MoreChildrenResponseData>))]
    [JsonSerializable(typeof(ApiResponse<FlairTemplate>))]
    [JsonSerializable(typeof(ApiResponse<LiveThreadCreateData>))]
    [JsonSerializable(typeof(SubmitRequest))]
    [JsonSerializable(typeof(RichTextDocument))]
    [JsonSerializable(typeof(MediaAssetResponse))]
    // Subreddits, users, multis
    [JsonSerializable(typeof(SubredditAutocompleteResponse))]
    [JsonSerializable(typeof(PostRequirements))]
    [JsonSerializable(typeof(SubredditRulesResponse))]
    [JsonSerializable(typeof(SubredditType))]
    [JsonSerializable(typeof(UserDataByIdsResponse))]
    [JsonSerializable(typeof(MultiResponse))]
    [JsonSerializable(typeof(List<MultiResponse>))]
    [JsonSerializable(typeof(MultiCreateRequest))]
    // Flair, emoji, live threads, moderation, modmail, widgets, wiki
    [JsonSerializable(typeof(FlairListResponse))]
    [JsonSerializable(typeof(List<FlairTemplate>))]
    [JsonSerializable(typeof(EmojisResponse))]
    [JsonSerializable(typeof(EmojiUploadLease))]
    [JsonSerializable(typeof(LiveThreadResponse))]
    [JsonSerializable(typeof(ModmailConversationsResponse))]
    [JsonSerializable(typeof(ModmailUnreadCount))]
    [JsonSerializable(typeof(Widget))]
    [JsonSerializable(typeof(WidgetsResponse))]
    [JsonSerializable(typeof(WikiPageResponse))]
    [JsonSerializable(typeof(WikiPagesResponse))]
    [JsonSerializable(typeof(WikiPageSettings))]
    internal partial class RedditJsonContext : JsonSerializerContext
    {
    }

    public static class RedditJson
    {
        /// <summary>
        /// The resolver Reddit.Api's options use: the generated contract first when run-time
        /// code generation isn't available, then reflection for anything it doesn't list.
        /// </summary>
        public static IJsonTypeInfoResolver TypeInfoResolver { get; } =
            System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported
                ? new DefaultJsonTypeInfoResolver()
                : JsonTypeInfoResolver.Combine(RedditJsonContext.Default, new DefaultJsonTypeInfoResolver());
    }
}
