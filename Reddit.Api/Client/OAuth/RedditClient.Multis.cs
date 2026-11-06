using Reddit.Api.Models.Json.Multis;
using System.Text.Json;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
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
        public async Task<MultiResponse?> GetMultiAsync(string multipath, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            // multipath should be like "user/username/m/multiname"
            return await this.GetAsync<MultiResponse>($"/api/multi/{multipath}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<MultiResponse>?> GetUserMultisAsync(string username, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<List<MultiResponse>>($"/api/multi/user/{username}", cancellationToken);
        }
    }
}