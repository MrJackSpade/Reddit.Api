using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.LinksComments
{
    /// <summary>
    /// Response from POST /api/submit.
    /// </summary>
    public class SubmitResponseData
    {
        [JsonPropertyName("drafts_count")]
        public int? DraftsCount { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}