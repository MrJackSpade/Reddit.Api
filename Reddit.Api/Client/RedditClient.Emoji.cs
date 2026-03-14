using Reddit.Api.Models.Json.Emoji;

namespace Reddit.Api.Client
{
    /// <summary>
    /// S3 upload lease for emoji.
    /// </summary>
    public class EmojiUploadLease
    {
        public Dictionary<string, string>? Fields { get; set; }

        public string? S3UploadLease { get; set; }
    }

    public partial class RedditClient
    {
        /// <summary>
        /// POST /api/v1/{subreddit}/emoji.json - Add a custom emoji.
        /// </summary>
        public async Task<bool> AddEmojiAsync(string subreddit, AddEmojiRequest request, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["name"] = request.Name,
                ["s3_key"] = request.S3Key,
                ["mod_flair_only"] = request.ModFlairOnly.ToString().ToLower(),
                ["post_flair_allowed"] = request.PostFlairAllowed.ToString().ToLower(),
                ["user_flair_allowed"] = request.UserFlairAllowed.ToString().ToLower()
            };

            return await this.PostFormAsync($"/api/v1/{subreddit}/emoji.json", formData, cancellationToken);
        }

        /// <summary>
        /// DELETE /api/v1/{subreddit}/emoji/{emoji_name} - Delete a custom emoji.
        /// </summary>
        public async Task<bool> DeleteEmojiAsync(string subreddit, string emojiName, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.DeleteAsync($"/api/v1/{subreddit}/emoji/{emojiName}", cancellationToken);
        }

        /// <summary>
        /// GET /api/v1/{subreddit}/emojis/all - Get all emojis for a subreddit.
        /// </summary>
        public async Task<EmojisResponse?> GetEmojisAsync(string subreddit, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            return await this.GetAsync<EmojisResponse>($"/api/v1/{subreddit}/emojis/all", cancellationToken);
        }

        /// <summary>
        /// POST /api/v1/{subreddit}/emoji_asset_upload_s3.json - Get S3 upload lease for emoji.
        /// </summary>
        public async Task<EmojiUploadLease?> GetEmojiUploadLeaseAsync(string subreddit, string filepath, string mimetype, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["filepath"] = filepath,
                ["mimetype"] = mimetype
            };

            return await this.PostFormAsync<EmojiUploadLease>($"/api/v1/{subreddit}/emoji_asset_upload_s3.json", formData, cancellationToken);
        }

        /// <summary>
        /// POST /api/v1/{subreddit}/emoji_custom_size - Set custom emoji size.
        /// </summary>
        public async Task<bool> SetEmojiCustomSizeAsync(string subreddit, int height, int width, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["height"] = height.ToString(),
                ["width"] = width.ToString()
            };

            return await this.PostFormAsync($"/api/v1/{subreddit}/emoji_custom_size", formData, cancellationToken);
        }
    }
}