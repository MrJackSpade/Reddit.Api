using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.LiveThreads;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <summary>
        /// GET /live/{thread} - Get live thread info.
        /// </summary>
        public async Task<LiveThreadResponse?> GetLiveThreadAsync(string threadId, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);
            return await GetAsync<LiveThreadResponse>($"/live/{threadId}/about", cancellationToken);
        }

        /// <summary>
        /// GET /live/{thread} - Get live thread updates.
        /// </summary>
        public async Task<Listing<Thing<LiveUpdate>>?> GetLiveUpdatesAsync(string threadId, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await TryAuthenticateAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<LiveUpdate>>>($"/live/{threadId}{query}", cancellationToken);
        }

        /// <summary>
        /// POST /api/live/create - Create a live thread.
        /// </summary>
        public async Task<LiveThreadResponse?> CreateLiveThreadAsync(CreateLiveThreadRequest request, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json",
                ["title"] = request.Title
            };

            if (!string.IsNullOrEmpty(request.Description))
                formData["description"] = request.Description;
            if (!string.IsNullOrEmpty(request.Resources))
                formData["resources"] = request.Resources;
            if (request.Nsfw.HasValue)
                formData["nsfw"] = request.Nsfw.Value.ToString().ToLower();

            var response = await PostFormAsync<ApiResponse<LiveThreadCreateData>>("/api/live/create", formData, cancellationToken);
            CheckApiResponse(response);

            if (response?.Json?.Data?.Id != null)
            {
                return await GetLiveThreadAsync(response.Json.Data.Id, cancellationToken);
            }

            return null;
        }

        /// <summary>
        /// POST /api/live/{thread}/update - Post update to live thread.
        /// </summary>
        public async Task<bool> PostLiveUpdateAsync(string threadId, string body, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json",
                ["body"] = body
            };

            return await PostFormAsync($"/api/live/{threadId}/update", formData, cancellationToken);
        }

        /// <summary>
        /// POST /api/live/{thread}/strike_update - Strike an update.
        /// </summary>
        public async Task<bool> StrikeLiveUpdateAsync(string threadId, string updateId, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json",
                ["id"] = updateId
            };

            return await PostFormAsync($"/api/live/{threadId}/strike_update", formData, cancellationToken);
        }

        /// <summary>
        /// POST /api/live/{thread}/delete_update - Delete an update.
        /// </summary>
        public async Task<bool> DeleteLiveUpdateAsync(string threadId, string updateId, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json",
                ["id"] = updateId
            };

            return await PostFormAsync($"/api/live/{threadId}/delete_update", formData, cancellationToken);
        }

        /// <summary>
        /// POST /api/live/{thread}/close_thread - Close the live thread.
        /// </summary>
        public async Task<bool> CloseLiveThreadAsync(string threadId, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json"
            };

            return await PostFormAsync($"/api/live/{threadId}/close_thread", formData, cancellationToken);
        }

        /// <summary>
        /// POST /api/live/{thread}/edit - Edit live thread settings.
        /// </summary>
        public async Task<bool> EditLiveThreadAsync(string threadId, string? title = null, string? description = null, string? resources = null, bool? nsfw = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json"
            };

            if (!string.IsNullOrEmpty(title))
                formData["title"] = title;
            if (!string.IsNullOrEmpty(description))
                formData["description"] = description;
            if (!string.IsNullOrEmpty(resources))
                formData["resources"] = resources;
            if (nsfw.HasValue)
                formData["nsfw"] = nsfw.Value.ToString().ToLower();

            return await PostFormAsync($"/api/live/{threadId}/edit", formData, cancellationToken);
        }
    }

    internal class LiveThreadCreateData
    {
        public string? Id { get; set; }
    }
}
