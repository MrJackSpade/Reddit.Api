using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Flair;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<List<FlairTemplate>?> GetLinkFlairAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);
            return await GetAsync<List<FlairTemplate>>($"/r/{subreddit}/api/link_flair_v2", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<FlairTemplate>?> GetUserFlairAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);
            return await GetAsync<List<FlairTemplate>>($"/r/{subreddit}/api/user_flair_v2", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> SelectFlairAsync(string subreddit, SelectFlairRequest request, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json"
            };

            if (!string.IsNullOrEmpty(request.Link))
                formData["link"] = request.Link;
            if (!string.IsNullOrEmpty(request.Name))
                formData["name"] = request.Name;
            if (!string.IsNullOrEmpty(request.FlairTemplateId))
                formData["flair_template_id"] = request.FlairTemplateId;
            if (!string.IsNullOrEmpty(request.Text))
                formData["text"] = request.Text;
            if (!string.IsNullOrEmpty(request.CssClass))
                formData["css_class"] = request.CssClass;
            if (!string.IsNullOrEmpty(request.BackgroundColor))
                formData["background_color"] = request.BackgroundColor;
            if (!string.IsNullOrEmpty(request.TextColor))
                formData["text_color"] = request.TextColor;

            return await PostFormAsync($"/r/{subreddit}/api/selectflair", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<FlairTemplate?> CreateFlairTemplateAsync(string subreddit, FlairTemplateRequest request, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json",
                ["flair_type"] = request.FlairType,
                ["text"] = request.Text
            };

            if (!string.IsNullOrEmpty(request.FlairTemplateId))
                formData["flair_template_id"] = request.FlairTemplateId;
            if (!string.IsNullOrEmpty(request.CssClass))
                formData["css_class"] = request.CssClass;
            if (!string.IsNullOrEmpty(request.TextColor))
                formData["text_color"] = request.TextColor;
            if (!string.IsNullOrEmpty(request.BackgroundColor))
                formData["background_color"] = request.BackgroundColor;
            if (request.TextEditable.HasValue)
                formData["text_editable"] = request.TextEditable.Value.ToString().ToLower();
            if (request.ModOnly.HasValue)
                formData["mod_only"] = request.ModOnly.Value.ToString().ToLower();
            if (!string.IsNullOrEmpty(request.AllowableContent))
                formData["allowable_content"] = request.AllowableContent;
            if (request.MaxEmojis.HasValue)
                formData["max_emojis"] = request.MaxEmojis.Value.ToString();
            if (request.OverrideCss.HasValue)
                formData["override_css"] = request.OverrideCss.Value.ToString().ToLower();

            var response = await PostFormAsync<ApiResponse<FlairTemplate>>($"/r/{subreddit}/api/flairtemplate_v2", formData, cancellationToken);
            CheckApiResponse(response);

            return response?.Json?.Data;
        }

        /// <inheritdoc />
        public async Task<FlairListResponse?> GetFlairListAsync(string subreddit, string? after = null, int? limit = null, string? name = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(after))
                queryParams.Add($"after={Uri.EscapeDataString(after)}");
            if (limit.HasValue)
                queryParams.Add($"limit={limit.Value}");
            if (!string.IsNullOrEmpty(name))
                queryParams.Add($"name={Uri.EscapeDataString(name)}");

            var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
            return await GetAsync<FlairListResponse>($"/r/{subreddit}/api/flairlist{query}", cancellationToken);
        }
    }
}
