namespace Reddit.Api.Client
{
    /// <summary>
    /// Selects which Reddit host a request is sent to. Each route's calls live in their own
    /// partial of <see cref="RedditClient"/> (Client/OAuth, Client/Www, Client/Old), and each of
    /// those partials documents why its calls are on that host.
    /// </summary>
    public enum RequestRoute
    {
        /// <summary>
        /// oauth.reddit.com with the bearer token.
        /// </summary>
        OAuth,

        /// <summary>
        /// www.reddit.com with the browser session cookies, when the session has them;
        /// otherwise the same as <see cref="OAuth"/>.
        /// </summary>
        Www,

        /// <summary>
        /// old.reddit.com with the browser session cookies, when the session has them;
        /// otherwise the same as <see cref="OAuth"/>.
        /// </summary>
        Old
    }
}
