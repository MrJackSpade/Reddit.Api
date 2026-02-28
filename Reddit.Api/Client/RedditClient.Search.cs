using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Search;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> SearchAsync(SearchParameters parameters, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);
            var query = parameters.ToQueryString();
            return await GetAsync<Listing<Thing<Link>>>($"/search{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> SearchSubredditAsync(string subreddit, SearchParameters parameters, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);

            // Add restrict_sr=true to limit search to the specified subreddit
            parameters.RestrictSr = true;
            var query = parameters.ToQueryString();

            return await GetAsync<Listing<Thing<Link>>>($"/r/{subreddit}/search{query}", cancellationToken);
        }
    }
}
