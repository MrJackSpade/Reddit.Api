namespace Reddit.Api.Exceptions
{
    /// <summary>
    /// A single structured error returned by the Reddit API.
    /// Reddit encodes each error as a positional array of [code, message, field].
    /// </summary>
    public sealed class RedditApiError
    {
        public RedditApiError(string code, string? message, string? field)
        {
            this.Code = code;
            this.Message = message;
            this.Field = field;
        }

        /// <summary>
        /// The error code, e.g. "THREAD_LOCKED", "RATELIMIT", "SUBREDDIT_NOTALLOWED".
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The human-readable message, e.g. "Comments are locked."
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// The form field the error applies to, if any (e.g. "parent", "text").
        /// </summary>
        public string? Field { get; }

        /// <summary>
        /// Builds a <see cref="RedditApiError"/> from Reddit's positional [code, message, field] array.
        /// </summary>
        public static RedditApiError FromArray(IReadOnlyList<string> parts)
        {
            string code = parts.Count > 0 ? parts[0] : "UNKNOWN";
            string? message = parts.Count > 1 ? parts[1] : null;
            string? field = parts.Count > 2 ? parts[2] : null;
            return new RedditApiError(code, message, field);
        }

        public override string ToString()
        {
            string text = string.IsNullOrEmpty(this.Message) ? this.Code : $"{this.Code}: {this.Message}";
            return string.IsNullOrEmpty(this.Field) ? text : $"{text} ({this.Field})";
        }
    }
}
