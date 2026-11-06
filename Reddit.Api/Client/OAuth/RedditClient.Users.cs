using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Users;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<bool> BlockUserAsync(string username, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["name"] = username
            };

            return await this.PostFormAsync("/api/block_user", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Thing<User>?> GetUserAboutAsync(string username, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<Thing<User>>($"/user/{username}/about", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Comment>>?> GetUserCommentsAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Comment>>>($"/user/{username}/comments{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<UserDataByIdsResponse?> GetUserDataByIdsAsync(IEnumerable<string> accountIds, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string ids = string.Join(",", accountIds);
            return await this.GetAsync<UserDataByIdsResponse>($"/api/user_data_by_account_ids?ids={ids}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetUserDownvotedAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"/user/{username}/downvoted{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<object>>?> GetUserOverviewAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<object>>>($"/user/{username}/overview{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<object>>?> GetUserSavedAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<object>>>($"/user/{username}/saved{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetUserSubmittedAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"/user/{username}/submitted{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetUserUpvotedAsync(string username, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"/user/{username}/upvoted{query}", cancellationToken);
        }
    }
}