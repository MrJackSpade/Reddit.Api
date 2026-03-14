using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Flair;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<FlairTemplate?> CreateFlairTemplateAsync(string subreddit, FlairTemplateRequest request, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["flair_type"] = request.FlairType.ToJsonString(),
                ["text"] = request.Text
            };

            if (!string.IsNullOrEmpty(request.FlairTemplateId))
            {
                formData["flair_template_id"] = request.FlairTemplateId;
            }

            if (!string.IsNullOrEmpty(request.CssClass))
            {
                formData["css_class"] = request.CssClass;
            }

            if (request.TextColor.HasValue)
            {
                formData["text_color"] = request.TextColor.Value;
            }

            if (request.BackgroundColor.HasValue)
            {
                formData["background_color"] = request.BackgroundColor.Value;
            }

            if (!request.TextEditable.IsNull)
            {
                formData["text_editable"] = request.TextEditable.ToString();
            }

            if (!request.ModOnly.IsNull)
            {
                formData["mod_only"] = request.ModOnly.ToString();
            }

            if (!string.IsNullOrEmpty(request.AllowableContent))
            {
                formData["allowable_content"] = request.AllowableContent;
            }

            if (request.MaxEmojis.HasValue)
            {
                formData["max_emojis"] = request.MaxEmojis.Value.ToString();
            }

            if (!request.OverrideCss.IsNull)
            {
                formData["override_css"] = request.OverrideCss.ToString();
            }

            ApiResponse<FlairTemplate>? response = await this.PostFormAsync<ApiResponse<FlairTemplate>>($"/r/{subreddit}/api/flairtemplate_v2", formData, cancellationToken);
            CheckApiResponse(response);

            return response?.Json?.Data;
        }

        /// <inheritdoc />
        public async Task<FlairListResponse?> GetFlairListAsync(string subreddit, string? after = null, int? limit = null, string? name = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            List<string> queryParams = new();

            if (!string.IsNullOrEmpty(after))
            {
                queryParams.Add($"after={Uri.EscapeDataString(after)}");
            }

            if (limit.HasValue)
            {
                queryParams.Add($"limit={limit.Value}");
            }

            if (!string.IsNullOrEmpty(name))
            {
                queryParams.Add($"name={Uri.EscapeDataString(name)}");
            }

            string query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
            return await this.GetAsync<FlairListResponse>($"/r/{subreddit}/api/flairlist{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<FlairTemplate>?> GetLinkFlairAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<List<FlairTemplate>>($"/r/{subreddit}/api/link_flair_v2", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<FlairTemplate>?> GetUserFlairAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<List<FlairTemplate>>($"/r/{subreddit}/api/user_flair_v2", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> SelectFlairAsync(string subreddit, SelectFlairRequest request, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json"
            };

            if (!string.IsNullOrEmpty(request.Link))
            {
                formData["link"] = request.Link;
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                formData["name"] = request.Name;
            }

            if (!string.IsNullOrEmpty(request.FlairTemplateId))
            {
                formData["flair_template_id"] = request.FlairTemplateId;
            }

            if (!string.IsNullOrEmpty(request.Text))
            {
                formData["text"] = request.Text;
            }

            if (!string.IsNullOrEmpty(request.CssClass))
            {
                formData["css_class"] = request.CssClass;
            }

            if (request.BackgroundColor.HasValue)
            {
                formData["background_color"] = request.BackgroundColor.Value;
            }

            if (request.TextColor.HasValue)
            {
                formData["text_color"] = request.TextColor.Value;
            }

            return await this.PostFormAsync($"/r/{subreddit}/api/selectflair", formData, cancellationToken);
        }
    }
}