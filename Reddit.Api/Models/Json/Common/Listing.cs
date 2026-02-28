using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Common
{
    /// <summary>
    /// Reddit's paginated listing wrapper.
    /// Used for any endpoint that returns multiple items with pagination support.
    /// </summary>
    public class Listing<T>
    {
        [JsonPropertyName("data")]
        public ListingData<T>? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "Listing";
    }

    /// <summary>
    /// Non-generic Listing for polymorphic scenarios.
    /// </summary>
    public class Listing
    {
        [JsonPropertyName("data")]
        public ListingData? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "Listing";
    }
}