using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Common
{
    /// <summary>
    /// The data payload inside a Listing wrapper.
    /// Contains pagination tokens and the list of children.
    /// </summary>
    public class ListingData<T>
    {
        /// <summary>
        /// Fullname of the listing anchor point (null for first page).
        /// </summary>
        [JsonPropertyName("after")]
        public string? After { get; set; }

        /// <summary>
        /// Fullname of the thing before the first item.
        /// </summary>
        [JsonPropertyName("before")]
        public string? Before { get; set; }

        /// <summary>
        /// Modhash for CSRF protection.
        /// </summary>
        [JsonPropertyName("modhash")]
        public string? Modhash { get; set; }

        /// <summary>
        /// Total distance from the first item (Reddit-specific).
        /// </summary>
        [JsonPropertyName("dist")]
        public int? Dist { get; set; }

        /// <summary>
        /// The list of Things in this listing.
        /// </summary>
        [JsonPropertyName("children")]
        public List<T> Children { get; set; } = [];

        /// <summary>
        /// Geolocation filter applied to the listing.
        /// </summary>
        [JsonPropertyName("geo_filter")]
        public string? GeoFilter { get; set; }
    }

    /// <summary>
    /// Non-generic ListingData for polymorphic scenarios.
    /// </summary>
    public class ListingData
    {
        [JsonPropertyName("after")]
        public string? After { get; set; }

        [JsonPropertyName("before")]
        public string? Before { get; set; }

        [JsonPropertyName("modhash")]
        public string? Modhash { get; set; }

        [JsonPropertyName("dist")]
        public int? Dist { get; set; }

        [JsonPropertyName("children")]
        public List<Thing>? Children { get; set; }

        [JsonPropertyName("geo_filter")]
        public string? GeoFilter { get; set; }
    }
}
