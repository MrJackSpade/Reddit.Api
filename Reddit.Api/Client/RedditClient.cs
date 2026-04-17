using Reddit.Api.Models;
using Reddit.Api.Models.Json.Account;
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

        private readonly JsonSerializerOptions _jsonOptions;

        private readonly SemaphoreSlim _throttleSemaphore = new(1, 1);

        private DateTime _lastRequest = DateTime.MinValue;

        private BearerToken? _token;

        private Func<Task<string?>>? _tokenRefreshFunc;

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
        public string? AuthenticatedUser => _token?.AuthenticatedUser;

        /// <inheritdoc />
        public bool CanAuthenticate => _credentials.IsValid;

        /// <inheritdoc />
        public bool IsAuthenticated => _token != null && !_token.IsExpired;

        /// <summary>
        /// Minimum delay between requests in milliseconds.
        /// Reddit recommends no more than 60 requests per minute.
        /// </summary>
        public int MinimumDelayMs { get; set; } = 500;

        /// <summary>
        /// Gets the base URL for API requests.
        /// </summary>
        protected string BaseUrl => IsAuthenticated ? OAuthBaseUrl : PublicBaseUrl;

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
                string authValue = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{_credentials.AppKey}:{_credentials.AppSecret}"));

                using HttpRequestMessage request = new(HttpMethod.Post, TokenUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                FormUrlEncodedContent formContent = new(new Dictionary<string, string>
                {
                    ["grant_type"] = "password",
                    ["username"] = _credentials.Username!,
                    ["password"] = _credentials.Password!
                });
                request.Content = formContent;

                HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                string content = await response.Content.ReadAsStringAsync(cancellationToken);
                OAuthToken? oauthResponse = JsonSerializer.Deserialize<OAuthToken>(content, _jsonOptions);

                if (oauthResponse == null || string.IsNullOrEmpty(oauthResponse.AccessToken) || _credentials.Username == null)
                {
                    return false;
                }

                // Set expiration 5 seconds before actual expiration for safety
                _token = new BearerToken(
                    oauthResponse.AccessToken,
                    _credentials.Username,
                    DateTime.UtcNow.AddSeconds(oauthResponse.ExpiresIn - 5));

                return true;
            }
            catch
            {
                _token = null;
                return false;
            }
        }

        /// <inheritdoc />
        public void SetTokenRefreshFunction(Func<Task<string?>> tokenRefreshFunc)
        {
            _tokenRefreshFunc = tokenRefreshFunc;
        }

        /// <inheritdoc />
        public void SetBearerToken(BearerToken token)
        {
            _token = token;
        }

        /// <inheritdoc />
        public async Task<bool> ValidateBearerToken(BearerToken token)
        {
            string? owner = await this.GetTokenOwner(token.AccessToken);
            return owner != null;
        }

        /// <inheritdoc />
        public async Task<string?> GetTokenOwner(string accessToken)
        {
            await this.ThrottleAsync(CancellationToken.None);

            using HttpRequestMessage request = new(HttpMethod.Get, $"{OAuthBaseUrl}/api/v1/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();
            MeResponse? me = JsonSerializer.Deserialize<MeResponse>(content, _jsonOptions);

            if (me is null)
            {
                throw new NullReferenceException("No owner returned for token");
            }

            return me.Name;

        }

        /// <inheritdoc />
        public void Dispose()
        {
            _throttleSemaphore.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Checks API response for errors and throws if found.
        /// </summary>
        protected static void CheckApiResponse<T>(ApiResponse<T>? response)
        {
            if (response?.Json?.Errors != null && response.Json.Errors.Count > 0)
            {
                List<string> errorMessages = response.Json.Errors
                    .Select(e => string.Join(": ", e))
                    .ToList();
                throw new InvalidOperationException(
                    $"Reddit API error: {string.Join("; ", errorMessages)}");
            }
        }

        /// <summary>
        /// Makes a DELETE request to the Reddit API.
        /// </summary>
        protected async Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = this.BuildUrl(endpoint);
            using HttpRequestMessage request = new(HttpMethod.Delete, url);

            HttpResponseMessage response = await this.SendWithAuthAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Ensures the client is authenticated before making a request.
        /// Falls back to the token refresh function if credentials aren't available.
        /// </summary>
        protected async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken)
        {
            if (IsAuthenticated)
            {
                return;
            }

            if (CanAuthenticate)
            {
                if (await this.AuthenticateAsync(cancellationToken))
                {
                    return;
                }
            }

            if (_tokenRefreshFunc != null)
            {
                string? token = await _tokenRefreshFunc();

                if (token != null)
                {
                    string? owner = await this.GetTokenOwner(token);
                    if (owner != null)
                    {
                        this.SetBearerToken(new BearerToken(token, owner));
                        return;
                    }
                }
            }

            throw new InvalidOperationException("Authentication required but failed.");
        }

        /// <summary>
        /// Makes a GET request to the Reddit API.
        /// </summary>
        protected async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = this.BuildUrl(endpoint);
            using HttpRequestMessage request = new(HttpMethod.Get, url);

            HttpResponseMessage response = await this.SendWithAuthAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Makes a PATCH request with JSON body to the Reddit API.
        /// </summary>
        protected async Task<T?> PatchJsonAsync<T>(string endpoint, object body, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = this.BuildUrl(endpoint);
            using HttpRequestMessage request = new(HttpMethod.Patch, url);

            string json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await this.SendWithAuthAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Makes a POST request with form data to the Reddit API.
        /// </summary>
        protected async Task<T?> PostFormAsync<T>(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = this.BuildUrl(endpoint);
            using HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await this.SendWithAuthAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Makes a POST request with form data, returning no content.
        /// </summary>
        protected async Task<bool> PostFormAsync(string endpoint, Dictionary<string, string> formData, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = this.BuildUrl(endpoint);
            using HttpRequestMessage request = new(HttpMethod.Post, url);
            request.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await this.SendWithAuthAsync(request, cancellationToken);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Makes a PUT request with JSON body to the Reddit API.
        /// </summary>
        protected async Task<T?> PutJsonAsync<T>(string endpoint, object body, CancellationToken cancellationToken)
        {
            await this.ThrottleAsync(cancellationToken);

            string url = this.BuildUrl(endpoint);
            using HttpRequestMessage request = new(HttpMethod.Put, url);

            string json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await this.SendWithAuthAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        /// <summary>
        /// Enforces rate limiting between requests.
        /// </summary>
        protected async Task ThrottleAsync(CancellationToken cancellationToken)
        {
            await _throttleSemaphore.WaitAsync(cancellationToken);
            try
            {
                TimeSpan elapsed = DateTime.UtcNow - _lastRequest;
                TimeSpan delay = TimeSpan.FromMilliseconds(MinimumDelayMs) - elapsed;
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
        /// Optionally authenticates if credentials or token refresh function are available.
        /// </summary>
        protected async Task TryAuthenticateAsync(CancellationToken cancellationToken)
        {
            if (IsAuthenticated)
            {
                return;
            }

            if (CanAuthenticate)
            {
                await this.AuthenticateAsync(cancellationToken);
                return;
            }

            if (_tokenRefreshFunc != null)
            {
                string? token = await _tokenRefreshFunc();

                if (token != null)
                {
                    string? owner = await this.GetTokenOwner(token);
                    if (owner != null)
                    {
                        this.SetBearerToken(new BearerToken(token, owner));
                    }
                }
            }
        }

        /// <summary>
        /// Sends a request and retries once with a refreshed token on 401 or 403.
        /// </summary>
        private async Task<HttpResponseMessage> SendWithAuthAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.AddAuthHeader(request);
            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            if (response.StatusCode is System.Net.HttpStatusCode.Unauthorized
                                    or System.Net.HttpStatusCode.Forbidden
                && _tokenRefreshFunc != null)
            {
                string? newToken = await _tokenRefreshFunc();

                if (newToken != null)
                {
                    string? owner = await this.GetTokenOwner(newToken);
                    if (owner == null)
                    {
                        return response;
                    }

                    this.SetBearerToken(new BearerToken(newToken, owner));

                    // Clone the request since the original is already sent/disposed
                    using HttpRequestMessage retry = new(request.Method, request.RequestUri);
                    this.AddAuthHeader(retry);

                    if (request.Content != null)
                    {
                        // Content may have been consumed; we need the caller to rebuild.
                        // For form/json content, read the bytes and recreate.
                        byte[] bytes = await request.Content.ReadAsByteArrayAsync(cancellationToken);
                        retry.Content = new ByteArrayContent(bytes);

                        if (request.Content.Headers.ContentType != null)
                        {
                            retry.Content.Headers.ContentType = request.Content.Headers.ContentType;
                        }
                    }

                    response = await _httpClient.SendAsync(retry, cancellationToken);
                }
            }

            return response;
        }

        /// <summary>
        /// Adds the OAuth authorization header to a request.
        /// </summary>
        private void AddAuthHeader(HttpRequestMessage request)
        {
            if (IsAuthenticated && _token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", _token.AccessToken);
            }
        }

        /// <summary>
        /// Builds the full URL for an endpoint.
        /// </summary>
        private string BuildUrl(string endpoint)
        {
            string url = $"{BaseUrl}{endpoint}";

            // Add .json extension for non-authenticated requests
            if (!IsAuthenticated && !endpoint.Contains('?'))
            {
                url += ".json";
            }
            else if (!IsAuthenticated && endpoint.Contains('?'))
            {
                string[] parts = endpoint.Split('?');
                url = $"{BaseUrl}{parts[0]}.json?{parts[1]}";
            }

            return url;
        }
    }
}