namespace Reddit.Api.Client
{
    /// <summary>
    /// Selects which Reddit host an authenticated request is sent to.
    /// </summary>
    public enum RequestRoute
    {
        /// <summary>
        /// oauth.reddit.com with the bearer token.
        /// </summary>
        Api,

        /// <summary>
        /// www.reddit.com JSON with the browser session cookies, when the session has them;
        /// otherwise the same as <see cref="Api"/>. Used for endpoints Reddit refuses to
        /// browser tokens on oauth.reddit.com (comment threads, /api/info).
        /// </summary>
        WebSession
    }
}
