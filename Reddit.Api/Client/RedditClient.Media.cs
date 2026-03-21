using Reddit.Api.Models.Json.Media;
using System.Net.Http.Headers;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <summary>
        /// POST /api/media/asset - Get an S3 upload lease for media.
        /// </summary>
        public async Task<MediaAssetResponse?> GetMediaAssetUploadLeaseAsync(string filepath, string mimetype, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["filepath"] = filepath,
                ["mimetype"] = mimetype
            };

            return await this.PostFormAsync<MediaAssetResponse>("/api/media/asset", formData, cancellationToken);
        }

        /// <summary>
        /// Upload media to S3 using an upload lease from GetMediaAssetUploadLeaseAsync.
        /// </summary>
        public async Task<bool> UploadMediaToS3Async(MediaAssetResponse lease, Stream fileStream, string filename, string mimetype, CancellationToken cancellationToken = default)
        {
            string actionUrl = lease.Args.Action;

            if (actionUrl.StartsWith("//"))
            {
                actionUrl = "https:" + actionUrl;
            }

            using MultipartFormDataContent content = new();

            // Add all lease fields first
            foreach (MediaAssetUploadField field in lease.Args.Fields)
            {
                content.Add(new StringContent(field.Value), field.Name);
            }

            // Add the file last
            StreamContent fileContent = new(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimetype);
            content.Add(fileContent, "file", filename);

            // Use a separate request without Reddit auth headers for S3
            using HttpRequestMessage request = new(HttpMethod.Post, actionUrl);
            request.Content = content;
            request.Headers.UserAgent.ParseAdd(_credentials.UserAgent);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            return response.StatusCode == System.Net.HttpStatusCode.Created ||
                   response.StatusCode == System.Net.HttpStatusCode.OK ||
                   response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
