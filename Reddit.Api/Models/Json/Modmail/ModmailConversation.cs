using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Modmail
{
    /// <summary>
    /// Modmail conversation state.
    /// </summary>
    public static class ModmailState
    {
        public const int Archived = 2;

        public const int InProgress = 1;

        public const int New = 0;
    }

    /// <summary>
    /// Request to create modmail conversation.
    /// </summary>
    public class CreateModmailRequest
    {
        /// <summary>
        /// Message body.
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Whether this is an internal mod discussion.
        /// </summary>
        public JsonBool IsAuthorHidden { get; set; }

        /// <summary>
        /// Subject line.
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Subreddit name.
        /// </summary>
        public string Subreddit { get; set; } = string.Empty;

        /// <summary>
        /// Recipient username.
        /// </summary>
        public string To { get; set; } = string.Empty;
    }

    public class ModmailAuthor
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("isHidden")]
        public bool IsHidden { get; set; }

        [JsonPropertyName("isMod")]
        public bool IsMod { get; set; }

        [JsonPropertyName("isOp")]
        public bool IsOp { get; set; }

        [JsonPropertyName("isParticipant")]
        public bool IsParticipant { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modmail conversation.
    /// </summary>
    public class ModmailConversation
    {
        [JsonPropertyName("authors")]
        public List<ModmailAuthor>? Authors { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("isAuto")]
        public bool IsAuto { get; set; }

        [JsonPropertyName("isHighlighted")]
        public bool IsHighlighted { get; set; }

        [JsonPropertyName("isInternal")]
        public bool IsInternal { get; set; }

        [JsonPropertyName("isRepliable")]
        public bool IsRepliable { get; set; }

        [JsonPropertyName("lastModUpdate")]
        public string? LastModUpdate { get; set; }

        [JsonPropertyName("lastUnread")]
        public string? LastUnread { get; set; }

        [JsonPropertyName("lastUpdated")]
        public string? LastUpdated { get; set; }

        [JsonPropertyName("lastUserUpdate")]
        public string? LastUserUpdate { get; set; }

        [JsonPropertyName("numMessages")]
        public int NumMessages { get; set; }

        [JsonPropertyName("objIds")]
        public List<ModmailObjId>? ObjIds { get; set; }

        [JsonPropertyName("owner")]
        public ModmailOwner? Owner { get; set; }

        [JsonPropertyName("participant")]
        public ModmailParticipant? Participant { get; set; }

        [JsonPropertyName("state")]
        public int State { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response from modmail conversation endpoints.
    /// </summary>
    public class ModmailConversationsResponse
    {
        [JsonPropertyName("conversationIds")]
        public List<string>? ConversationIds { get; set; }

        [JsonPropertyName("conversations")]
        public Dictionary<string, ModmailConversation>? Conversations { get; set; }

        [JsonPropertyName("messages")]
        public Dictionary<string, ModmailMessage>? Messages { get; set; }

        [JsonPropertyName("viewerId")]
        public string? ViewerId { get; set; }
    }

    /// <summary>
    /// Modmail message.
    /// </summary>
    public class ModmailMessage
    {
        [JsonPropertyName("author")]
        public ModmailAuthor? Author { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; } = string.Empty;

        [JsonPropertyName("bodyMarkdown")]
        public string? BodyMarkdown { get; set; }

        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("isInternal")]
        public bool IsInternal { get; set; }
    }

    public class ModmailObjId
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;
    }

    public class ModmailOwner
    {
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    public class ModmailParticipant
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("isHidden")]
        public bool IsHidden { get; set; }

        [JsonPropertyName("isMod")]
        public bool IsMod { get; set; }

        [JsonPropertyName("isOp")]
        public bool IsOp { get; set; }

        [JsonPropertyName("isParticipant")]
        public bool IsParticipant { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}