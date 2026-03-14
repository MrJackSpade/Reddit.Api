using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Account
{
    /// <summary>
    /// Blocked user information from /api/v1/me/blocked.
    /// </summary>
    public class BlockedUserInfo
    {
        [JsonPropertyName("date")]
        public double Date { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("rel_id")]
        public string? RelId { get; set; }
    }

    /// <summary>
    /// Friend information from /api/v1/me/friends.
    /// </summary>
    public class FriendInfo
    {
        [JsonPropertyName("date")]
        public double Date { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("rel_id")]
        public string? RelId { get; set; }
    }

    public class UserListData
    {
        [JsonPropertyName("children")]
        public List<FriendInfo> Children { get; set; } = [];
    }

    /// <summary>
    /// Response wrapper for user list (friends, blocked).
    /// </summary>
    public class UserListResponse
    {
        [JsonPropertyName("data")]
        public UserListData? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "UserList";
    }
}