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
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["api_type"] = "json",
                ["to"] = to,
                ["subject"] = subject,
                ["text"] = body
            };

            if (!string.IsNullOrEmpty(fromSr))
                formData["from_sr"] = fromSr;

            var response = await PostFormAsync<ApiResponse<object>>("/api/compose", formData, cancellationToken);
            CheckApiResponse(response);

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteMessageAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["id"] = fullname
            };

            return await PostFormAsync("/api/del_msg", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> ReadMessageAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["id"] = fullname
            };

            return await PostFormAsync("/api/read_message", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> UnreadMessageAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>
            {
                ["id"] = fullname
            };

            return await PostFormAsync("/api/unread_message", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> ReadAllMessagesAsync(CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            var formData = new Dictionary<string, string>();

            return await PostFormAsync("/api/read_all_messages", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Message>>?> GetInboxAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<Message>>>($"/message/inbox{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Message>>?> GetSentAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<Message>>>($"/message/sent{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Message>>?> GetUnreadAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            var query = parameters?.ToQueryString() ?? string.Empty;
            return await GetAsync<Listing<Thing<Message>>>($"/message/unread{query}", cancellationToken);
        }
    }
}
