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
        /// old.reddit.com JSON with the browser session cookies, when the session has them;
        /// otherwise the same as <see cref="Api"/>. Used for endpoints Reddit refuses to
        /// browser tokens on oauth.reddit.com (comment threads, /api/info, /api/morechildren,
        /// search). old.reddit.com because www.reddit.com refuses /api/ paths to the session.
        /// </summary>
        WebSession
    }
}
