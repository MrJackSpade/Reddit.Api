using Reddit.Api.Client;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    /// <summary>
    /// The fixture is a www thread page as a logged-in WebView had it (r/NintendoSwitch 1xpfn3x):
    /// the post and its menu, and the comment tree, with scripts, styles and menus removed.
    /// </summary>
    [TestClass]
    public class ShredditThreadParserTests
    {
        [TestMethod]
        public void ParsePost_ReadsThePagesPost()
        {
            Link post = ShredditThreadParser.ParsePost(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "shreddit-thread-page.html")));

            Assert.AreEqual("t3_1xpfn3x", post.Name);
            Assert.AreEqual("NintendoSwitch", post.Subreddit);
            Assert.AreEqual(133, post.NumComments);
            Assert.AreEqual("https://i.redd.it/kvb53eolg00i1.jpeg", post.Url);
            Assert.IsTrue(post.SelftextHtml!.Length > 0);
        }

        [TestMethod]
        public void ParsePost_RejectsAPageThatIsntAThread()
        {
            // What a request for the page gets without passing Reddit's JavaScript challenge.
            const string shell = "<html><head><title>Reddit - The heart of the internet</title></head><body><script>/* js_challenge */</script></body></html>";

            Assert.ThrowsException<InvalidOperationException>(() => ShredditThreadParser.ParsePost(shell));
        }
    }
}
