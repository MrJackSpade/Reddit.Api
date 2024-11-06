using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Api
{
    public class GalleryData
    {
        [JsonPropertyName("items")]
        public List<Item> Items { get; init; } = [];
    }
}