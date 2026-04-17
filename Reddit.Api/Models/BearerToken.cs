namespace Reddit.Api.Models
{
    /// <summary>
    /// A validated bearer token session. The access token has been confirmed valid
    /// and the authenticated user resolved. Expiration is optional — browser-extracted
    /// tokens (e.g. token_v2 cookie) don't carry an expiry, so callers rely on a 403
    /// to detect invalidation.
    /// </summary>
    public record BearerToken(string AccessToken, string AuthenticatedUser, DateTime? Expiration = null)
    {
        /// <summary>
        /// True if the token has a known expiration that has passed.
        /// Tokens without an expiration are never considered expired here —
        /// invalidation is detected via a 403 response.
        /// </summary>
        public bool IsExpired => Expiration != null && DateTime.UtcNow >= Expiration;
    }
}
