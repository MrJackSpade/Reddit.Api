using Reddit.Api.Models.Json.Modmail;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Unread modmail count response.
    /// </summary>
    public class ModmailUnreadCount
    {
        public int Archived { get; set; }

        public int Highlighted { get; set; }

        public int InProgress { get; set; }

        public int Mod { get; set; }

        public int New { get; set; }

        public int Notifications { get; set; }
    }

    public partial class RedditClient
    {
        /// <summary>
        /// POST /api/mod/conversations/{conversation_id}/archive - Archive a conversation.
        /// </summary>
        public async Task<bool> ArchiveModmailAsync(string conversationId, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.PostFormAsync($"/api/mod/conversations/{conversationId}/archive", new Dictionary<string, string>(), cancellationToken);
        }

        /// <summary>
        /// POST /api/mod/conversations - Create a modmail conversation.
        /// </summary>
        public async Task<ModmailConversationsResponse?> CreateModmailConversationAsync(CreateModmailRequest request, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["to"] = request.To,
                ["subject"] = request.Subject,
                ["body"] = request.Body,
                ["srName"] = request.Subreddit
            };

            if (!request.IsAuthorHidden.IsNull)
            {
                formData["isAuthorHidden"] = request.IsAuthorHidden.ToString();
            }

            return await this.PostFormAsync<ModmailConversationsResponse>("/api/mod/conversations", formData, cancellationToken);
        }

        /// <summary>
        /// GET /api/mod/conversations/{conversation_id} - Get a specific conversation.
        /// </summary>
        public async Task<ModmailConversationsResponse?> GetModmailConversationAsync(string conversationId, bool markRead = false, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            string query = markRead ? "?markRead=true" : string.Empty;
            return await this.GetAsync<ModmailConversationsResponse>($"/api/mod/conversations/{conversationId}{query}", cancellationToken);
        }

        /// <summary>
        /// GET /api/mod/conversations - Get modmail conversations.
        /// </summary>
        public async Task<ModmailConversationsResponse?> GetModmailConversationsAsync(
            string? subreddits = null,
            string? state = null,
            string? sort = null,
            string? after = null,
            int? limit = null,
            CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            string query = new QueryStringBuilder()
                .Add("entity", subreddits)
                .Add("state", state)
                .Add("sort", sort)
                .Add("after", after)
                .Add("limit", limit)
                .Build();
            return await this.GetAsync<ModmailConversationsResponse>($"/api/mod/conversations{query}", cancellationToken);
        }

        /// <summary>
        /// GET /api/mod/conversations/unread/count - Get unread conversation count.
        /// </summary>
        public async Task<ModmailUnreadCount?> GetModmailUnreadCountAsync(CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.GetAsync<ModmailUnreadCount>("/api/mod/conversations/unread/count", cancellationToken);
        }

        /// <summary>
        /// POST /api/mod/conversations/{conversation_id}/highlight - Highlight a conversation.
        /// </summary>
        public async Task<bool> HighlightModmailAsync(string conversationId, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.PostFormAsync($"/api/mod/conversations/{conversationId}/highlight", new Dictionary<string, string>(), cancellationToken);
        }

        /// <summary>
        /// POST /api/mod/conversations/read - Mark conversations as read.
        /// </summary>
        public async Task<bool> MarkModmailReadAsync(IEnumerable<string> conversationIds, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["conversationIds"] = string.Join(",", conversationIds)
            };

            return await this.PostFormAsync("/api/mod/conversations/read", formData, cancellationToken);
        }

        /// <summary>
        /// POST /api/mod/conversations/unread - Mark conversations as unread.
        /// </summary>
        public async Task<bool> MarkModmailUnreadAsync(IEnumerable<string> conversationIds, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["conversationIds"] = string.Join(",", conversationIds)
            };

            return await this.PostFormAsync("/api/mod/conversations/unread", formData, cancellationToken);
        }

        /// <summary>
        /// POST /api/mod/conversations/{conversation_id} - Reply to a modmail conversation.
        /// </summary>
        public async Task<ModmailConversationsResponse?> ReplyToModmailAsync(string conversationId, string body, bool isInternal = false, bool isAuthorHidden = false, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["body"] = body,
                ["isInternal"] = isInternal.ToString().ToLower(),
                ["isAuthorHidden"] = isAuthorHidden.ToString().ToLower()
            };

            return await this.PostFormAsync<ModmailConversationsResponse>($"/api/mod/conversations/{conversationId}", formData, cancellationToken);
        }

        /// <summary>
        /// POST /api/mod/conversations/{conversation_id}/unarchive - Unarchive a conversation.
        /// </summary>
        public async Task<bool> UnarchiveModmailAsync(string conversationId, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.PostFormAsync($"/api/mod/conversations/{conversationId}/unarchive", new Dictionary<string, string>(), cancellationToken);
        }

        /// <summary>
        /// DELETE /api/mod/conversations/{conversation_id}/highlight - Remove highlight from conversation.
        /// </summary>
        public async Task<bool> UnhighlightModmailAsync(string conversationId, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.DeleteAsync($"/api/mod/conversations/{conversationId}/highlight", cancellationToken);
        }
    }
}