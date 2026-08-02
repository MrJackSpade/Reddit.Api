namespace Reddit.Api.Exceptions
{
    /// <summary>
    /// Base type for all errors originating from the Reddit API client.
    /// Catch this to handle any Reddit-side failure uniformly.
    /// </summary>
    public abstract class RedditException : Exception
    {
        protected RedditException(string message) : base(message)
        {
        }

        protected RedditException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
