namespace Reddit.Api.Exceptions
{
    /// <summary>
    /// Thrown when an operation requires an authenticated user but the client
    /// is not logged in and could not obtain a token. Callers can catch this
    /// specifically to prompt the user to sign in, rather than treating it as
    /// a generic failure.
    /// </summary>
    public sealed class RedditAuthenticationException : RedditException
    {
        public RedditAuthenticationException(string message) : base(message)
        {
        }

        public RedditAuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
