using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Media
{
    /// <summary>
    /// Response from POST /api/media/asset.
    /// </summary>
    public class MediaAssetResponse
    {
        [JsonPropertyName("args")]
        public MediaAssetUploadArgs Args { get; set; } = new();

        [JsonPropertyName("asset")]
        public MediaAssetInfo Asset { get; set; } = new();
    }

    /// <summary>
    /// Upload arguments containing the S3 action URL and form fields.
    /// </summary>
    public class MediaAssetUploadArgs
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("fields")]
        public List<MediaAssetUploadField> Fields { get; set; } = [];
    }

    /// <summary>
    /// A name/value pair representing an S3 form field.
    /// </summary>
    public class MediaAssetUploadField
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
    }

    /// <summary>
    /// Asset information returned after creating an upload lease.
    /// </summary>
    public class MediaAssetInfo
    {
        [JsonPropertyName("asset_id")]
        public string AssetId { get; set; } = string.Empty;

        [JsonPropertyName("processing_state")]
        public string ProcessingState { get; set; } = string.Empty;

        [JsonPropertyName("payload")]
        public MediaAssetPayload? Payload { get; set; }

        [JsonPropertyName("websocket_url")]
        public string? WebsocketUrl { get; set; }
    }

    /// <summary>
    /// Payload information for the media asset.
    /// </summary>
    public class MediaAssetPayload
    {
        [JsonPropertyName("filepath")]
        public string? Filepath { get; set; }
    }
}
