namespace Reddit.Api.Client
{
    /// <summary>
    /// The calls sent to oauth.reddit.com with the bearer token: every partial in Client/OAuth.
    /// This route has a deadline: see "Where things are moving" below.
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
    /// - Reddit is closing the public API: it announced on 2026-09-30 that access starts closing on
    ///   2027-01-12 for apps and users that haven't registered, and closes for everyone in March 2027.
    ///   Deaddit won't register, so everything here has to be off this route before January.
    /// - When something breaks, check www first, for parity with what Reddit's main site serves.
    ///   old.reddit.com lacks some state, and is itself being limited to recent users and mods.
    /// - Probe with a throwaway test using the stored session before moving anything, and move a
    ///   call between partials rather than adding a route override inside a feature file.
    ///
    /// What's still here that the app uses, and why:
    /// - GetMediaAssetUploadLeaseAsync (images in comments): /api/media/asset answers 403 on www and
    ///   old with the session and modhash. It needs the site's real upload path found. BLOCKS MARCH.
    /// - ComposeMessageAsync (DMs): /api/compose is blocked on www. Reddit replaced PMs with chat, and
    ///   here it answers RESTRICTED_TO_PM for the accounts tried, so "Message" may already be dead.
    ///
    /// Everything else here is library surface the app doesn't call (moderation, modmail, wiki,
    /// widgets, live threads, emoji, flair, and the typed listing helpers). It hasn't been tested
    /// live, so it may already be blocked, and it will stop in March if not moved.
    ///
    /// /api/unread_message answers 404 here: Reddit retired it with PMs. The app only marks things read.
    /// </summary>
    public partial class RedditClient
    {
    }
}
