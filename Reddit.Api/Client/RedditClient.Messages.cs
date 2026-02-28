using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Messages;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<bool> ComposeMessageAsync(string to, string subject, string body, string? fromSr = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["to"] = to,
                ["subject"] = subject,
                ["text"] = body
            };

            if (!string.IsNullOrEmpty(fromSr))
            {
                formData["from_sr"] = fromSr;
            }

            ApiResponse<object>? response = await this.PostFormAsync<ApiResponse<object>>("/api/compose", formData, cancellationToken);
            CheckApiResponse(response);

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteMessageAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/del_msg", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Message>>?> GetInboxAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Message>>>($"/message/inbox{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Message>>?> GetSentAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Message>>>($"/message/sent{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Message>>?> GetUnreadAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Message>>>($"/message/unread{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> ReadAllMessagesAsync(CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new();

            return await this.PostFormAsync("/api/read_all_messages", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> ReadMessageAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/read_message", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> UnreadMessageAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unread_message", formData, cancellationToken);
        }
    }
}