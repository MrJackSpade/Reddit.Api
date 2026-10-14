namespace Reddit.Api.Models
{
    /// <summary>
    /// A validated bearer token session. The access token has been confirmed valid
    /// and the authenticated user resolved.
    /// </summary>
    /// <param name="AccessToken">The bearer token sent to oauth.reddit.com.</param>
    /// <param name="AuthenticatedUser">The username that owns the token.</param>
    /// <param name="Expiration">When the token stops being accepted, if known.</param>
    /// <param name="SessionCookies">
    /// The complete reddit.com <c>Cookie</c> header of the browser session the token came from
    /// (token_v2 logins only). Reddit's network security refuses comment threads to browser
    /// tokens on oauth.reddit.com, but serves them on www.reddit.com to requests carrying the
    /// full browser session, so <see cref="Client.RequestRoute.WebSession"/> endpoints use this.
    /// </param>
    public record BearerToken(string AccessToken, string AuthenticatedUser, DateTime? Expiration = null, string? SessionCookies = null)
    {
        /// <summary>
        /// True if the token has a known expiration that has passed.
        /// </summary>
        public bool IsExpired => Expiration != null && DateTime.UtcNow >= Expiration;
    }
}
