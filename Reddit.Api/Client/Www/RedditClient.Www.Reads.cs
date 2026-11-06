using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Media;
using Reddit.Api.Models.Json.Messages;
using Reddit.Api.Models.Json.Moderation;
using Reddit.Api.Models.Json.Multis;
using Reddit.Api.Models.Json.Subreddits;
using Reddit.Api.Models.Json.Users;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Reddit.Api.Client
{
    /// <summary>
    /// The app's lookups, sent to www.reddit.com with the session cookies. See RedditClient.Www.cs for the context.
    /// </summary>
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<List<MultiResponse>?> GetMyMultisAsync(CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.GetAsync<List<MultiResponse>>("/api/multi/mine", cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<Thing<Subreddit>?> GetSubredditAboutAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<Thing<Subreddit>>($"/r/{subreddit}/about", cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<SubredditRulesResponse?> GetSubredditRulesAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<SubredditRulesResponse>($"/r/{subreddit}/about/rules", cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<Thing<User>?> GetUserAboutAsync(string username, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<Thing<User>>($"/user/{username}/about", cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<UserDataByIdsResponse?> GetUserDataByIdsAsync(IEnumerable<string> accountIds, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string ids = string.Join(",", accountIds);
            return await this.GetAsync<UserDataByIdsResponse>($"/api/user_data_by_account_ids?ids={ids}", cancellationToken, RequestRoute.Www);
        }
    }
}
