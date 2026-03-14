using Reddit.Api.Converters;
using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Messages
{
    /// <summary>
    /// Reddit private message (t4) data.
    /// </summary>
    public class Message
    {
        [JsonPropertyName("associated_awarding_id")]
        public string? AssociatedAwardingId { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }

        [JsonPropertyName("author_fullname")]
        public string? AuthorFullname { get; set; }

        [JsonPropertyName("body")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? Body { get; set; }

        [JsonPropertyName("body_html")]
        [JsonConverter(typeof(HtmlDecodedStringConverter))]
        public string? BodyHtml { get; set; }

        [JsonPropertyName("context")]
        public string? Context { get; set; }

        [JsonPropertyName("created")]
        public JsonDateTime Created { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("dest")]
        public string? Dest { get; set; }

        [JsonPropertyName("distinguished")]
        public DistinguishedKind Distinguished { get; set; }

        [JsonPropertyName("first_message")]
        public long? FirstMessage { get; set; }

        [JsonPropertyName("first_message_name")]
        public string? FirstMessageName { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("new")]
        public bool IsNew { get; set; }

        [JsonPropertyName("likes")]
        public VoteState Likes { get; set; }

        [JsonPropertyName("link_title")]
        public string? LinkTitle { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("num_comments")]
        public int? NumComments { get; set; }

        [JsonPropertyName("parent_id")]
        public string? ParentId { get; set; }

        [JsonPropertyName("replies")]
        public object? Replies { get; set; }

        [JsonPropertyName("score")]
        public int? Score { get; set; }

        [JsonPropertyName("subject")]
        public string? Subject { get; set; }

        [JsonPropertyName("subreddit")]
        public string? Subreddit { get; set; }

        [JsonPropertyName("subreddit_name_prefixed")]
        public string? SubredditNamePrefixed { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("was_comment")]
        public bool WasComment { get; set; }

        // Can be empty string or listing
    }
}