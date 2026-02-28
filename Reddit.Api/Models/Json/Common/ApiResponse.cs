using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Common
{
    /// <summary>
    /// Standard Reddit API response wrapper used for mutation operations.
    /// Format: { "json": { "errors": [], "data": {...} } }
    /// </summary>
    public class ApiResponse<T>
    {
        [JsonPropertyName("json")]
        public ApiResponseJson<T>? Json { get; set; }
    }

    /// <summary>
    /// Non-data API response (for operations that don't return data).
    /// </summary>
    public class ApiResponse
    {
        [JsonPropertyName("json")]
        public ApiResponseJsonNoData? Json { get; set; }
    }

    /// <summary>
    /// The JSON envelope inside an API response.
    /// </summary>
    public class ApiResponseJson<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<List<string>> Errors { get; set; } = [];
    }

    /// <summary>
    /// API response JSON with no data field.
    /// </summary>
    public class ApiResponseJsonNoData
    {
        [JsonPropertyName("errors")]
        public List<List<string>> Errors { get; set; } = [];
    }

    /// <summary>
    /// Generic error response from Reddit API.
    /// </summary>
    public class ErrorResponse
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }
}