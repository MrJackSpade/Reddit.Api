using Reddit.Api.Models.Json.Widgets;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <summary>
        /// POST /r/{subreddit}/api/widget - Create a widget.
        /// </summary>
        public async Task<Widget?> CreateWidgetAsync(string subreddit, Widget widget, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.PostJsonAsync<Widget>($"/r/{subreddit}/api/widget", widget, cancellationToken);
        }

        /// <summary>
        /// DELETE /r/{subreddit}/api/widget/{widget_id} - Delete a widget.
        /// </summary>
        public async Task<bool> DeleteWidgetAsync(string subreddit, string widgetId, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.DeleteAsync($"/r/{subreddit}/api/widget/{widgetId}", cancellationToken);
        }

        /// <summary>
        /// GET /r/{subreddit}/api/widgets - Get subreddit widgets.
        /// </summary>
        public async Task<WidgetsResponse?> GetWidgetsAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<WidgetsResponse>($"/r/{subreddit}/api/widgets", cancellationToken);
        }

        /// <summary>
        /// PUT /r/{subreddit}/api/widget/{widget_id} - Update a widget.
        /// </summary>
        public async Task<Widget?> UpdateWidgetAsync(string subreddit, string widgetId, Widget widget, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.PutJsonAsync<Widget>($"/r/{subreddit}/api/widget/{widgetId}", widget, cancellationToken);
        }

        /// <summary>
        /// PATCH /r/{subreddit}/api/widget_order/{section} - Update widget order.
        /// </summary>
        public async Task<bool> UpdateWidgetOrderAsync(string subreddit, string section, List<string> widgetIds, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            var body = new { order = widgetIds };
            object? response = await this.PatchJsonAsync<object>($"/r/{subreddit}/api/widget_order/{section}", body, cancellationToken);
            return response != null;
        }

        /// <summary>
        /// Makes a POST request with JSON body.
        /// </summary>
        private async Task<T?> PostJsonAsync<T>(string endpoint, object body, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = $"{BaseUrl}{endpoint}";
            using HttpRequestMessage request = new(HttpMethod.Post, url);
            this.AddAuthHeader(request);

            string json = System.Text.Json.JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            return System.Text.Json.JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
    }
}