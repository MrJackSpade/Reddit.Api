using System.Net;

namespace Reddit.Api.Exceptions
{
    /// <summary>
    /// Thrown when the Reddit API responds with a non-success HTTP status code
    /// that is not accompanied by a structured error envelope. Carries the
    /// status code, request URL, and raw response body for diagnostics.
    /// </summary>
    public sealed class RedditHttpException : RedditException
    {
        public RedditHttpException(HttpStatusCode statusCode, string url, string? responseBody)
            : base(BuildMessage(statusCode, url))
        {
            this.StatusCode = statusCode;
            this.Url = url;
            this.ResponseBody = responseBody;
        }

        /// <summary>
        /// The HTTP status code returned by Reddit.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The request URL that produced the failure.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// The raw response body, if any was returned.
        /// </summary>
        public string? ResponseBody { get; }

        private static string BuildMessage(HttpStatusCode statusCode, string url)
        {
            return $"Reddit API request to '{url}' failed with status {(int)statusCode} ({statusCode}).";
        }
    }
}
