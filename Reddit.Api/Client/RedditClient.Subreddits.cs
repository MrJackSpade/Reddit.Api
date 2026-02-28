using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Subreddits;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<Thing<Subreddit>?> GetSubredditAboutAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);
            return await GetAsync<Thing<Subreddit>>($"/r/{subreddit}/about", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<SubredditRulesResponse?> GetSubredditRulesAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);
            return await GetAsync<SubredditRulesResponse>($"/r/{subreddit}/about/rules", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> SubscribeAsync(string subreddit, bool subscribe, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["action"] = subscribe ? SubscribeAction.Subscribe : SubscribeAction.Unsubscribe,
                ["sr_name"] = subreddit
            };

            return await PostFormAsync("/api/subscribe", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Subreddit>>?> GetMySubscribedAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<Subreddit>>>($"/subreddits/mine/subscriber{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Subreddit>>?> GetMyModeratedAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<Subreddit>>>($"/subreddits/mine/moderator{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Subreddit>>?> SearchSubredditsAsync(string query, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);

            var queryString = parameters?.ToQueryString() ?? string.Empty;
            if (string.IsNullOrEmpty(queryString))
            {
                queryString = $"?q={Uri.EscapeDataString(query)}";
            }
            else
            {
                queryString += $"&q={Uri.EscapeDataString(query)}";
            }

            return await GetAsync<Listing<Thing<Subreddit>>>($"/subreddits/search{queryString}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<SubredditAutocompleteItem>?> AutocompleteSubredditsAsync(
            string query,
            bool includeOver18 = false,
            bool includeProfiles = false,
            CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);

            var queryParams = new List<string>
            {
                $"query={Uri.EscapeDataString(query)}",
                $"include_over_18={includeOver18.ToString().ToLower()}",
                $"include_profiles={includeProfiles.ToString().ToLower()}"
            };

            var queryString = "?" + string.Join("&", queryParams);
            var response = await GetAsync<SubredditAutocompleteResponse>($"/api/subreddit_autocomplete{queryString}", cancellationToken);

            return response?.Subreddits;
        }

        /// <inheritdoc />
        public async Task<PostRequirements?> GetPostRequirementsAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await GetAsync<PostRequirements>($"/api/v1/{subreddit}/post_requirements", cancellationToken);
        }
    }

    // Helper response class
    internal class SubredditAutocompleteResponse
    {
        [JsonPropertyName("subreddits")]
        public List<SubredditAutocompleteItem>? Subreddits { get; set; }
    }
}
