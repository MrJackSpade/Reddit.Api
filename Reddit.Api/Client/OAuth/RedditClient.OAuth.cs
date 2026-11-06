namespace Reddit.Api.Client
{
    /// <summary>
    /// The calls sent to oauth.reddit.com with the bearer token: every partial in Client/OAuth.
    ///
    /// CONTEXT: why the client is split by host.
    /// Reddit is clamping down on automated access (LLM scraping, as far as we can tell), and
    /// Deaddit, an accessibility client used with a real account, keeps getting caught in it.
    /// Blocks arrive as a 403 HTML page reading "You've been blocked by network security", applied
    /// per host and per path, and they do not depend on the user agent or the TLS fingerprint:
    /// impersonating Chrome with curl-impersonate gets the same page. Each breakage used to get
    /// patched inside whichever feature file held the call, which hid what was on which host. So
    /// the calls are now grouped by the host they use.
    ///
    /// What has happened so far (dates are when we found out):
    /// - Browser tokens were refused comment threads, /api/info and search on OAuth, so those
    ///   moved to www with the browser session cookies.
    /// - 2026-10-23: www refused /api/ paths to the session, so they moved to old.reddit.com.
    /// - 2026-11-03: /api/morechildren.json was blocked everywhere; the suffix-less HTML form works.
    /// - 2026-11-06: /api/info was blocked on every host and form, /by_id, /search and the "new"
    ///   sort were blocked everywhere as JSON, and threads were still refused on OAuth.
    ///
    /// Where things are moving:
    /// - When something breaks, check www first, for parity with what Reddit's main site serves.
    ///   old.reddit.com lacks some state (it doesn't track comment reply state, for one).
    /// - Probe with a throwaway test using the stored session before moving anything, and move a
    ///   call between partials rather than adding a route override inside a feature file.
    /// - This partial is what still works with the bearer token: listings, inbox, every write
    ///   (vote, save, comment, submit, message...), multis and user/subreddit lookups. A call only
    ///   leaves it when Reddit blocks it here.
    ///
    /// Status of what's left here: the calls the app uses were tested live on 2026-11-06. The
    /// calls the app does not use (moderation, modmail, wiki, widgets, live threads, emoji, flair)
    /// were not, so they may already be blocked.
    /// </summary>
    public partial class RedditClient
    {
    }
}
