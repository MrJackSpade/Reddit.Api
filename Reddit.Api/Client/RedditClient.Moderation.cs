using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Moderation;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<Listing<Thing<object>>?> GetModQueueAsync(string subreddit, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<object>>>($"/r/{subreddit}/about/modqueue{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<object>>?> GetReportsAsync(string subreddit, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<object>>>($"/r/{subreddit}/about/reports{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<object>>?> GetSpamAsync(string subreddit, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<object>>>($"/r/{subreddit}/about/spam{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<object>>?> GetEditedAsync(string subreddit, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<object>>>($"/r/{subreddit}/about/edited{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<ModAction>>?> GetModLogAsync(
            string subreddit,
            ListingParameters? parameters = null,
            string? type = null,
            string? mod = null,
            CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var queryParams = new List<string>();

            if (parameters != null)
            {
                var paramQuery = parameters.ToQueryString().TrimStart('?');
                if (!string.IsNullOrEmpty(paramQuery))
                    queryParams.Add(paramQuery);
            }

            if (!string.IsNullOrEmpty(type))
                queryParams.Add($"type={Uri.EscapeDataString(type)}");
            if (!string.IsNullOrEmpty(mod))
                queryParams.Add($"mod={Uri.EscapeDataString(mod)}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
            return await GetAsync<Listing<Thing<ModAction>>>($"/r/{subreddit}/about/log{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> ApproveAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["id"] = fullname
            };

            return await PostFormAsync("/api/approve", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(string fullname, bool spam = false, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["id"] = fullname,
                ["spam"] = spam.ToString().ToLower()
            };

            return await PostFormAsync("/api/remove", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Thing<Comment>?> DistinguishAsync(string fullname, string how = "yes", bool? sticky = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json",
                ["id"] = fullname,
                ["how"] = how
            };

            if (sticky.HasValue)
                formData["sticky"] = sticky.Value.ToString().ToLower();

            var response = await PostFormAsync<ApiResponse<CommentResponseData>>("/api/distinguish", formData, cancellationToken);
            CheckApiResponse(response);

            return response?.Json?.Data?.Things?.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<bool> IgnoreReportsAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["id"] = fullname
            };

            return await PostFormAsync("/api/ignore_reports", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> UnignoreReportsAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["id"] = fullname
            };

            return await PostFormAsync("/api/unignore_reports", formData, cancellationToken);
        }
    }
}
