using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Subreddits;
using System.Text.Json.Serialization;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<List<SubredditAutocompleteItem>?> AutocompleteSubredditsAsync(
            string query,
            bool includeOver18 = false,
            bool includeProfiles = false,
            CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            List<string> queryParams = new()
            {
                $"query={Uri.EscapeDataString(query)}",
                $"include_over_18={includeOver18.ToString().ToLower()}",
                $"include_profiles={includeProfiles.ToString().ToLower()}"
            };

            string queryString = "?" + string.Join("&", queryParams);
            SubredditAutocompleteResponse? response = await this.GetAsync<SubredditAutocompleteResponse>($"/api/subreddit_autocomplete{queryString}", cancellationToken);

            return response?.Subreddits;
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Subreddit>>?> GetMyModeratedAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Subreddit>>>($"/subreddits/mine/moderator{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Subreddit>>?> GetMySubscribedAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Subreddit>>>($"/subreddits/mine/subscriber{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PostRequirements?> GetPostRequirementsAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.GetAsync<PostRequirements>($"/api/v1/{subreddit}/post_requirements", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Thing<Subreddit>?> GetSubredditAboutAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<Thing<Subreddit>>($"/r/{subreddit}/about", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<SubredditRulesResponse?> GetSubredditRulesAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<SubredditRulesResponse>($"/r/{subreddit}/about/rules", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Subreddit>>?> SearchSubredditsAsync(string query, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            string queryString = parameters?.ToQueryString() ?? string.Empty;
            if (string.IsNullOrEmpty(queryString))
            {
                queryString = $"?q={Uri.EscapeDataString(query)}";
            }
            else
            {
                queryString += $"&q={Uri.EscapeDataString(query)}";
            }

            return await this.GetAsync<Listing<Thing<Subreddit>>>($"/subreddits/search{queryString}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> SubscribeAsync(string subreddit, bool subscribe, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["action"] = subscribe ? SubscribeAction.Subscribe : SubscribeAction.Unsubscribe,
                ["sr_name"] = subreddit
            };

            return await this.PostFormAsync("/api/subscribe", formData, cancellationToken);
        }
    }

    // Helper response class
    internal class SubredditAutocompleteResponse
    {
        [JsonPropertyName("subreddits")]
        public List<SubredditAutocompleteItem>? Subreddits { get; set; }
    }
}