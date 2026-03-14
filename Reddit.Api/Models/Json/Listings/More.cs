using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Listings
{
    /// <summary>
    /// Reddit "more" placeholder for collapsed comments.
    /// </summary>
    public class More
    {
        [JsonPropertyName("children")]
        public List<string> Children { get; set; } = [];

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("depth")]
        public int Depth { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; } = string.Empty;
    }
}