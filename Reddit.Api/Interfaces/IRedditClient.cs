﻿using Reddit.Api.Models;
using Reddit.Api.Models.Api;
using Reddit.Api.Models.Requests;
using Reddit.Api.Models.ThingDefinitions;

namespace Reddit.Api.Interfaces
{
    public interface IRedditClient
    {
        bool CanLogIn { get; }

        bool IsLoggedIn { get; }

        public string? LoggedInUser { get; }

        Task<ApiSubReddit> About(SubRedditDefinition subreddit);

        Task<ApiComment> Comment(ApiThing replyTo, string comment);

        Task<List<ApiThing>> Comments(ApiPost thing, ApiComment? focus);

        Task Delete(ApiThing thing);

        Task<Dictionary<string, UserPartial>> GetPartialUserData(IEnumerable<string> usernames);

        Task<ApiPost> GetPost(string id);

        Task<List<ApiThing>> GetPosts<T>(ApiEndpointDefinition endpointDefinition, T sort, int pageSize, string? after = null, Region region = Region.GLOBAL) where T : Enum;

        Task<Stream> GetStream(string url);

        Task<ApiUser> GetUserData(string username);

        Task MarkRead(ApiThing message, bool state);

        Task Message(ApiUser thing, string subject, string body);

        Task<List<ApiThing>> MoreComments(ApiPost thing, IMore comment);

        Task<List<ApiMulti>> Multis();

        Task SetUpvoteState(ApiThing thing, UpvoteState state);

        Task ToggleInboxReplies(ApiThing thing, bool enabled);

        Task ToggleSave(ApiThing thing, bool saved);

        Task ToggleSubScription(ApiSubReddit thing, bool subscribed);

        Task ToggleVisibility(ApiThing thing, bool visible);

        Task<ApiComment> Update(ApiThing thing);
    }
}