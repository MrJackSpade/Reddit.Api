using Reddit.Api.Models.Json.Widgets;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <summary>
        /// GET /r/{subreddit}/api/widgets - Get subreddit widgets.
        /// </summary>
        public async Task<WidgetsResponse?> GetWidgetsAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);
            return await GetAsync<WidgetsResponse>($"/r/{subreddit}/api/widgets", cancellationToken);
        }

        /// <summary>
        /// POST /r/{subreddit}/api/widget - Create a widget.
        /// </summary>
        public async Task<Widget?> CreateWidgetAsync(string subreddit, Widget widget, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await PostJsonAsync<Widget>($"/r/{subreddit}/api/widget", widget, cancellationToken);
        }

        /// <summary>
        /// PUT /r/{subreddit}/api/widget/{widget_id} - Update a widget.
        /// </summary>
        public async Task<Widget?> UpdateWidgetAsync(string subreddit, string widgetId, Widget widget, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await PutJsonAsync<Widget>($"/r/{subreddit}/api/widget/{widgetId}", widget, cancellationToken);
        }

        /// <summary>
        /// DELETE /r/{subreddit}/api/widget/{widget_id} - Delete a widget.
        /// </summary>
        public async Task<bool> DeleteWidgetAsync(string subreddit, string widgetId, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await DeleteAsync($"/r/{subreddit}/api/widget/{widgetId}", cancellationToken);
        }

        /// <summary>
        /// PATCH /r/{subreddit}/api/widget_order/{section} - Update widget order.
        /// </summary>
        public async Task<bool> UpdateWidgetOrderAsync(string subreddit, string section, List<string> widgetIds, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var body = new { order = widgetIds };
            var response = await PatchJsonAsync<object>($"/r/{subreddit}/api/widget_order/{section}", body, cancellationToken);
            return response != null;
        }

        /// <summary>
        /// Makes a POST request with JSON body.
        /// </summary>
        private async Task<T?> PostJsonAsync<T>(string endpoint, object body, CancellationToken cancellationToken)
        {
            await ThrottleAsync(cancellationToken);

            var url = $"{BaseUrl}{endpoint}";
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddAuthHeader(request);

            var json = System.Text.Json.JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return System.Text.Json.JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}
