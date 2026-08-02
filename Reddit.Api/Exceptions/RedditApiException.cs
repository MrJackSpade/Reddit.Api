namespace Reddit.Api.Exceptions
{
    /// <summary>
    /// Thrown when the Reddit API returns a well-formed error envelope
    /// (<c>{ "json": { "errors": [[code, message, field], ...] } }</c>).
    /// Carries the structured errors so callers can react to specific codes
    /// (e.g. THREAD_LOCKED, RATELIMIT) rather than parsing message strings.
    /// </summary>
    public sealed class RedditApiException : RedditException
    {
        public RedditApiException(IReadOnlyList<RedditApiError> errors)
            : base(BuildMessage(errors))
        {
            this.Errors = errors;
        }

        /// <summary>
        /// The structured errors reported by Reddit. Always contains at least one entry.
        /// </summary>
        public IReadOnlyList<RedditApiError> Errors { get; }

        /// <summary>
        /// The first error's code, provided for convenience when only one error is expected.
        /// </summary>
        public string Code => this.Errors[0].Code;

        /// <summary>
        /// Returns true if any of the reported errors has the given code (case-insensitive).
        /// </summary>
        public bool HasCode(string code)
        {
            foreach (RedditApiError error in this.Errors)
            {
                if (string.Equals(error.Code, code, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string BuildMessage(IReadOnlyList<RedditApiError> errors)
        {
            if (errors == null || errors.Count == 0)
            {
                return "Reddit API error.";
            }

            return string.Join("; ", errors.Select(e => e.ToString()));
        }
    }
}
