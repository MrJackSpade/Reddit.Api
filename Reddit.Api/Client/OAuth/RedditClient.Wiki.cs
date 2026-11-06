using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Wiki;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <summary>
        /// POST /r/{subreddit}/api/wiki/edit - Edit a wiki page.
        /// </summary>
        public async Task<bool> EditWikiPageAsync(string subreddit, string page, string content, string? reason = null, string? previousRevision = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["page"] = page,
                ["content"] = content
            };

            if (!string.IsNullOrEmpty(reason))
            {
                formData["reason"] = reason;
            }

            if (!string.IsNullOrEmpty(previousRevision))
            {
                formData["previous"] = previousRevision;
            }

            return await this.PostFormAsync($"/r/{subreddit}/api/wiki/edit", formData, cancellationToken);
        }

        /// <summary>
        /// GET /r/{subreddit}/wiki/{page} - Get wiki page content.
        /// </summary>
        public async Task<WikiPageResponse?> GetWikiPageAsync(string subreddit, string page, string? revision = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string query = !string.IsNullOrEmpty(revision) ? $"?v={revision}" : string.Empty;
            return await this.GetAsync<WikiPageResponse>($"/r/{subreddit}/wiki/{page}{query}", cancellationToken);
        }

        /// <summary>
        /// GET /r/{subreddit}/wiki/pages - Get list of wiki pages.
        /// </summary>
        public async Task<WikiPagesResponse?> GetWikiPagesAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<WikiPagesResponse>($"/r/{subreddit}/wiki/pages", cancellationToken);
        }

        /// <summary>
        /// GET /r/{subreddit}/wiki/revisions/{page} - Get wiki page revisions.
        /// </summary>
        public async Task<Listing<Thing<WikiRevision>>?> GetWikiRevisionsAsync(string subreddit, string page, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<WikiRevision>>>($"/r/{subreddit}/wiki/revisions/{page}{query}", cancellationToken);
        }

        /// <summary>
        /// GET /r/{subreddit}/wiki/settings/{page} - Get wiki page settings.
        /// </summary>
        public async Task<WikiPageSettings?> GetWikiSettingsAsync(string subreddit, string page, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.GetAsync<WikiPageSettings>($"/r/{subreddit}/wiki/settings/{page}", cancellationToken);
        }

        /// <summary>
        /// POST /r/{subreddit}/api/wiki/hide - Toggle revision visibility.
        /// </summary>
        public async Task<bool> HideWikiRevisionAsync(string subreddit, string page, string revision, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["page"] = page,
                ["revision"] = revision
            };

            return await this.PostFormAsync($"/r/{subreddit}/api/wiki/hide", formData, cancellationToken);
        }

        /// <summary>
        /// POST /r/{subreddit}/api/wiki/revert - Revert wiki page to revision.
        /// </summary>
        public async Task<bool> RevertWikiPageAsync(string subreddit, string page, string revision, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["page"] = page,
                ["revision"] = revision
            };

            return await this.PostFormAsync($"/r/{subreddit}/api/wiki/revert", formData, cancellationToken);
        }

        /// <summary>
        /// POST /r/{subreddit}/api/wiki/alloweditor/{act} - Add/remove wiki editors.
        /// </summary>
        public async Task<bool> SetWikiEditorAsync(string subreddit, string page, string username, bool allow, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            string action = allow ? "add" : "del";
            Dictionary<string, string> formData = new()
            {
                ["page"] = page,
                ["username"] = username
            };

            return await this.PostFormAsync($"/r/{subreddit}/api/wiki/alloweditor/{action}", formData, cancellationToken);
        }
    }
}