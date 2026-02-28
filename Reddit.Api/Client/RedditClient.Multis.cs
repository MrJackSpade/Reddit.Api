using Reddit.Api.Models.Json.Multis;
using System.Text.Json;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<bool> AddSubredditToMultiAsync(string multipath, string subreddit, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            var model = new { name = subreddit };
            Dictionary<string, string> formData = new()
            {
                ["model"] = JsonSerializer.Serialize(model, _jsonOptions)
            };

            return await this.PutFormAsync($"/api/multi/{multipath}/r/{subreddit}", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<MultiResponse?> CopyMultiAsync(string from, string to, string? displayName = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["from"] = from,
                ["to"] = to
            };

            if (!string.IsNullOrEmpty(displayName))
            {
                formData["display_name"] = displayName;
            }

            return await this.PostFormAsync<MultiResponse>("/api/multi/copy", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<MultiResponse?> CreateOrUpdateMultiAsync(string multipath, MultiCreateRequest request, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            // Reddit expects the model as a JSON string in a form parameter
            var model = new
            {
                display_name = request.DisplayName,
                description_md = request.DescriptionMd,
                visibility = request.Visibility,
                key_color = request.KeyColor,
                icon_name = request.IconName,
                weighting_scheme = request.WeightingScheme,
                subreddits = request.Subreddits?.Select(s => new { name = s.Name }).ToList()
            };

            Dictionary<string, string> formData = new()
            {
                ["model"] = JsonSerializer.Serialize(model, _jsonOptions)
            };

            return await this.PutFormAsync<MultiResponse>($"/api/multi/{multipath}", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteMultiAsync(string multipath, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.DeleteAsync($"/api/multi/{multipath}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<MultiResponse?> GetMultiAsync(string multipath, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            // multipath should be like "user/username/m/multiname"
            return await this.GetAsync<MultiResponse>($"/api/multi/{multipath}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<MultiResponse>?> GetMyMultisAsync(CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.GetAsync<List<MultiResponse>>("/api/multi/mine", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<MultiResponse>?> GetUserMultisAsync(string username, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<List<MultiResponse>>($"/api/multi/user/{username}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveSubredditFromMultiAsync(string multipath, string subreddit, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.DeleteAsync($"/api/multi/{multipath}/r/{subreddit}", cancellationToken);
        }

        /// <summary>
        /// Makes a PUT request with form data to the Reddit API.
        /// </summary>
        private async Task<T?> PutFormAsync<T>(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = $"{BaseUrl}{endpoint}";
            using HttpRequestMessage request = new(HttpMethod.Put, url);
            this.AddAuthHeader(request);
            request.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Makes a PUT request with form data, returning boolean success.
        /// </summary>
        private async Task<bool> PutFormAsync(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = $"{BaseUrl}{endpoint}";
            using HttpRequestMessage request = new(HttpMethod.Put, url);
            this.AddAuthHeader(request);
            request.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}