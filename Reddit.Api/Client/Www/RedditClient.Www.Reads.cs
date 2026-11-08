using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Media;
using Reddit.Api.Models.Json.Messages;
using Reddit.Api.Models.Json.Moderation;
using Reddit.Api.Models.Json.Multis;
using Reddit.Api.Models.Json.Subreddits;
using Reddit.Api.Models.Json.Search;
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
        public async Task<Listing<Thing<Link>>?> SearchAsync(SearchParameters parameters, CancellationToken cancellationToken = default)
        {
            return await this.SearchPartialAsync("/svc/shreddit/search/", parameters, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> SearchSubredditAsync(string subreddit, SearchParameters parameters, CancellationToken cancellationToken = default)
        {
            return await this.SearchPartialAsync($"/svc/shreddit/r/{Uri.EscapeDataString(subreddit)}/search/", parameters, cancellationToken);
        }

        /// <summary>
        /// Search through www's results partial, which gives the matching post IDs (7 to a page) and
        /// the next page's cursor, returned as the listing's After and taken back as
        /// <see cref="SearchParameters.After"/>. The posts come from www, one request each.
        /// </summary>
        private async Task<Listing<Thing<Link>>?> SearchPartialAsync(string path, SearchParameters parameters, CancellationToken cancellationToken)
        {
            await this.EnsureWebSessionAsync("Search", cancellationToken);

            string query = new QueryStringBuilder()
                .Add("q", parameters.Query)
                .Add("type", "posts")
                .Add("sort", parameters.Sort?.ToJsonString())
                .Add("t", parameters.Time)
                .Add("cursor", parameters.After)
                .Build();

            (List<string> names, string? cursor) = ShredditListingParser.ParseSearch(await this.GetPartialAsync($"{path}{query}", cancellationToken));

            Listing<Thing<Link>>? posts = await this.GetByIdAsync(names, cancellationToken);
            posts!.Data!.After = cursor;
            return posts;
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
