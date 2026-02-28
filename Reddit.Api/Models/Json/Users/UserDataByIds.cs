using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Users
{
    /// <summary>
    /// Response from GET /api/user_data_by_account_ids.
    /// Maps account IDs to partial user data.
    /// </summary>
    public class UserDataByIdsResponse : Dictionary<string, UserPartialData>
    {
    }

    /// <summary>
    /// Partial user data returned by bulk lookup.
    /// </summary>
    public class UserPartialData
    {
        [JsonPropertyName("comment_karma")]
        public int CommentKarma { get; set; }

        [JsonPropertyName("created_utc")]
        public JsonDateTime CreatedUtc { get; set; }

        [JsonPropertyName("link_karma")]
        public int LinkKarma { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("profile_color")]
        public JsonColor ProfileColor { get; set; }

        [JsonPropertyName("profile_img")]
        public string? ProfileImg { get; set; }

        [JsonPropertyName("profile_over_18")]
        public JsonBool ProfileOver18 { get; set; }
    }
}