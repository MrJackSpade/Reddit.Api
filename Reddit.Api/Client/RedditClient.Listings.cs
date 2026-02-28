using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;
using System.Text.Json;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetBestAsync(ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"/best{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetByIdAsync(IEnumerable<string> fullnames, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string names = string.Join(",", fullnames);
            return await this.GetAsync<Listing<Thing<Link>>>($"/by_id/{names}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<(Thing<Link>? Post, Listing<Thing<Comment>>? Comments)> GetCommentsAsync(
            string articleId,
            string? commentId = null,
            string? sort = null,
            int? limit = null,
            int? depth = null,
            CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            List<string> queryParams = new();
            if (!string.IsNullOrEmpty(commentId))
            {
                queryParams.Add($"comment={commentId}");
            }

            if (!string.IsNullOrEmpty(sort))
            {
                queryParams.Add($"sort={sort}");
            }

            if (limit.HasValue)
            {
                queryParams.Add($"limit={limit.Value}");
            }

            if (depth.HasValue)
            {
                queryParams.Add($"depth={depth.Value}");
            }

            string query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
            string endpoint = $"/comments/{articleId}{query}";

            // Reddit returns an array of two listings: [post, comments]
            JsonElement result = await this.GetAsync<JsonElement>(endpoint, cancellationToken);

            if (result.ValueKind != JsonValueKind.Array)
            {
                return (null, null);
            }

            Thing<Link>? post = null;
            Listing<Thing<Comment>>? comments = null;

            List<JsonElement> elements = result.EnumerateArray().ToList();
            if (elements.Count >= 1)
            {
                Listing<Thing<Link>>? postListing = JsonSerializer.Deserialize<Listing<Thing<Link>>>(elements[0].GetRawText(), _jsonOptions);
                post = postListing?.Data?.Children?.FirstOrDefault();
            }

            if (elements.Count >= 2)
            {
                comments = JsonSerializer.Deserialize<Listing<Thing<Comment>>>(elements[1].GetRawText(), _jsonOptions);
            }

            return (post, comments);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetControversialAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string endpoint = string.IsNullOrEmpty(subreddit) ? "/controversial" : $"/r/{subreddit}/controversial";
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"{endpoint}{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<(Thing<Link>? Post, Listing<Thing<Link>>? Duplicates)> GetDuplicatesAsync(
            string articleId,
            ListingParameters? parameters = null,
            CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            string query = parameters?.ToQueryString() ?? string.Empty;
            string endpoint = $"/duplicates/{articleId}{query}";

            // Reddit returns an array of two listings: [original post, duplicates]
            JsonElement result = await this.GetAsync<JsonElement>(endpoint, cancellationToken);

            if (result.ValueKind != JsonValueKind.Array)
            {
                return (null, null);
            }

            Thing<Link>? post = null;
            Listing<Thing<Link>>? duplicates = null;

            List<JsonElement> elements = result.EnumerateArray().ToList();
            if (elements.Count >= 1)
            {
                Listing<Thing<Link>>? postListing = JsonSerializer.Deserialize<Listing<Thing<Link>>>(elements[0].GetRawText(), _jsonOptions);
                post = postListing?.Data?.Children?.FirstOrDefault();
            }

            if (elements.Count >= 2)
            {
                duplicates = JsonSerializer.Deserialize<Listing<Thing<Link>>>(elements[1].GetRawText(), _jsonOptions);
            }

            return (post, duplicates);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetHotAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string endpoint = string.IsNullOrEmpty(subreddit) ? "/hot" : $"/r/{subreddit}/hot";
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"{endpoint}{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetNewAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string endpoint = string.IsNullOrEmpty(subreddit) ? "/new" : $"/r/{subreddit}/new";
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"{endpoint}{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetRisingAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string endpoint = string.IsNullOrEmpty(subreddit) ? "/rising" : $"/r/{subreddit}/rising";
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"{endpoint}{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetTopAsync(string? subreddit = null, ListingParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);
            string endpoint = string.IsNullOrEmpty(subreddit) ? "/top" : $"/r/{subreddit}/top";
            string query = parameters?.ToQueryString() ?? string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"{endpoint}{query}", cancellationToken);
        }
    }
}