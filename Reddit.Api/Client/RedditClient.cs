using Reddit.Api.Models.Json.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Reddit API client implementation.
    /// Base class handles authentication, HTTP requests, and rate limiting.
    /// </summary>
    public partial class RedditClient : IRedditClient, IDisposable
    {
        private const string OAuthBaseUrl = "https://oauth.reddit.com";
        private const string PublicBaseUrl = "https://www.reddit.com";
        private const string TokenUrl = "https://www.reddit.com/api/v1/access_token";

        private readonly RedditCredentials _credentials;
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _throttleSemaphore = new(1, 1);
        private readonly JsonSerializerOptions _jsonOptions;

        private OAuthToken? _token;
        private DateTime _tokenExpiration = DateTime.MinValue;
        private DateTime _lastRequest = DateTime.MinValue;

        /// <summary>
        /// Minimum delay between requests in milliseconds.
        /// Reddit recommends no more than 60 requests per minute.
        /// </summary>
        public int MinimumDelayMs { get; set; } = 500;

        /// <inheritdoc />
        public bool CanAuthenticate => _credentials.IsValid;

        /// <inheritdoc />
        public bool IsAuthenticated => _token != null && DateTime.UtcNow < _tokenExpiration;

        /// <inheritdoc />
        public string? AuthenticatedUser { get; private set; }

        /// <summary>
        /// Creates a new RedditClient instance.
        /// </summary>
        /// <param name="credentials">Reddit API credentials.</param>
        /// <param name="httpClient">Optional HttpClient instance.</param>
        public RedditClient(RedditCredentials credentials, HttpClient? httpClient = null)
        {
            _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_credentials.UserAgent);

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <inheritdoc />
        public async Task<bool> AuthenticateAsync(CancellationToken cancellationToken = default)
        {
            if (!CanAuthenticate)
            {
                return false;
            }

            // Check if we already have a valid token
            if (IsAuthenticated)
            {
                return true;
            }

            try
            {
                // Create Basic auth header
                var authValue = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{_credentials.AppKey}:{_credentials.AppSecret}"));

                using var request = new HttpRequestMessage(HttpMethod.Post, TokenUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "password",
                    ["username"] = _credentials.Username!,
                    ["password"] = _credentials.Password!
                });
                request.Content = formContent;

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _token = JsonSerializer.Deserialize<OAuthToken>(content, _jsonOptions);

                if (_token == null || string.IsNullOrEmpty(_token.AccessToken))
                {
                    return false;
                }

                // Set expiration 5 seconds before actual expiration for safety
                _tokenExpiration = DateTime.UtcNow.AddSeconds(_token.ExpiresIn - 5);
                AuthenticatedUser = _credentials.Username;

                return true;
            }
            catch
            {
                _token = null;
                AuthenticatedUser = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the base URL for API requests.
        /// </summary>
        protected string BaseUrl => IsAuthenticated ? OAuthBaseUrl : PublicBaseUrl;

        /// <summary>
        /// Ensures the client is authenticated before making a request.
        /// </summary>
        protected async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken)
        {
            if (!IsAuthenticated)
            {
                var success = await AuthenticateAsync(cancellationToken);
                if (!success)
                {
                    throw new InvalidOperationException("Authentication required but failed.");
                }
            }
        }

        /// <summary>
        /// Optionally authenticates if credentials are available.
        /// </summary>
        protected async Task TryAuthenticateAsync(CancellationToken cancellationToken)
        {
            if (!IsAuthenticated && CanAuthenticate)
            {
                await AuthenticateAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Enforces rate limiting between requests.
        /// </summary>
        protected async Task ThrottleAsync(CancellationToken cancellationToken)
        {
            await _throttleSemaphore.WaitAsync(cancellationToken);
            try
            {
                var elapsed = DateTime.UtcNow - _lastRequest;
                var delay = TimeSpan.FromMilliseconds(MinimumDelayMs) - elapsed;
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay, cancellationToken);
                }
                _lastRequest = DateTime.UtcNow;
            }
            finally
            {
                _throttleSemaphore.Release();
            }
        }

        /// <summary>
        /// Makes a GET request to the Reddit API.
        /// </summary>
        protected async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            await ThrottleAsync(cancellationToken);

            var url = BuildUrl(endpoint);
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            AddAuthHeader(request);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Makes a POST request with form data to the Reddit API.
        /// </summary>
        protected async Task<T?> PostFormAsync<T>(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken)
        {
            await ThrottleAsync(cancellationToken);

            var url = BuildUrl(endpoint);
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddAuthHeader(request);
            request.Content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Makes a POST request with form data, returning no content.
        /// </summary>
        protected async Task<bool> PostFormAsync(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken)
        {
            await ThrottleAsync(cancellationToken);

            var url = BuildUrl(endpoint);
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddAuthHeader(request);
            request.Content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Makes a PATCH request with JSON body to the Reddit API.
        /// </summary>
        protected async Task<T?> PatchJsonAsync<T>(string endpoint, object body, CancellationToken cancellationToken)
        {
            await ThrottleAsync(cancellationToken);

            var url = BuildUrl(endpoint);
            using var request = new HttpRequestMessage(HttpMethod.Patch, url);
            AddAuthHeader(request);

            var json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Makes a PUT request with JSON body to the Reddit API.
        /// </summary>
        protected async Task<T?> PutJsonAsync<T>(string endpoint, object body, CancellationToken cancellationToken)
        {
            await ThrottleAsync(cancellationToken);

            var url = BuildUrl(endpoint);
            using var request = new HttpRequestMessage(HttpMethod.Put, url);
            AddAuthHeader(request);

            var json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Makes a DELETE request to the Reddit API.
        /// </summary>
        protected async Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken)
        {
            await ThrottleAsync(cancellationToken);

            var url = BuildUrl(endpoint);
            using var request = new HttpRequestMessage(HttpMethod.Delete, url);
            AddAuthHeader(request);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Builds the full URL for an endpoint.
        /// </summary>
        private string BuildUrl(string endpoint)
        {
            var url = $"{BaseUrl}{endpoint}";

            // Add .json extension for non-authenticated requests
            if (!IsAuthenticated && !endpoint.Contains('?'))
            {
                url += ".json";
            }
            else if (!IsAuthenticated && endpoint.Contains('?'))
            {
                var parts = endpoint.Split('?');
                url = $"{BaseUrl}{parts[0]}.json?{parts[1]}";
            }

            return url;
        }

        /// <summary>
        /// Adds the OAuth authorization header to a request.
        /// </summary>
        private void AddAuthHeader(HttpRequestMessage request)
        {
            if (IsAuthenticated && _token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    _token.TokenType, _token.AccessToken);
            }
        }

        /// <summary>
        /// Checks API response for errors and throws if found.
        /// </summary>
        protected static void CheckApiResponse<T>(ApiResponse<T>? response)
        {
            if (response?.Json?.Errors != null && response.Json.Errors.Count > 0)
            {
                var errorMessages = response.Json.Errors
                    .Select(e => string.Join(": ", e))
                    .ToList();
                throw new InvalidOperationException(
                    $"Reddit API error: {string.Join("; ", errorMessages)}");
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _throttleSemaphore.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
