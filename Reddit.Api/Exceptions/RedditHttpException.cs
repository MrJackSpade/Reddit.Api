using System.Net;
using System.Text.RegularExpressions;

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
            : base(BuildMessage(statusCode, url, responseBody))
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

        private static string BuildMessage(HttpStatusCode statusCode, string url, string? responseBody)
        {
            string message = $"Reddit API request to '{url}' failed with status {(int)statusCode} ({statusCode}).";
            string? reason = DescribeBody(responseBody);
            return reason == null ? message : $"{message} Reddit responded: {reason}";
        }

        /// <summary>
        /// What Reddit said in a failed response. Reddit's block pages are HTML, so for HTML this
        /// is the page's title, or its visible text when it has none; otherwise the body itself.
        /// Whitespace is collapsed either way.
        /// </summary>
        private static string? DescribeBody(string? responseBody)
        {
            if (string.IsNullOrWhiteSpace(responseBody))
            {
                return null;
            }

            const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            string text = responseBody;

            if (Regex.IsMatch(text, @"<(html|body|div|head)\b", Options))
            {
                Match title = Regex.Match(text, @"<title[^>]*>(.*?)</title>", Options);

                text = title.Success
                    ? title.Groups[1].Value
                    : Regex.Replace(Regex.Replace(text, @"<(style|script)\b.*?</\1>", " ", Options), @"<[^>]+>", " ");

                text = WebUtility.HtmlDecode(text);
            }

            return Regex.Replace(text, @"\s+", " ").Trim();
        }
    }
}
