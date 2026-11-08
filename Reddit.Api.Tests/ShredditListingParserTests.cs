using Reddit.Api.Client;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    /// <summary>
    /// Fixtures are www.reddit.com's listing and search partials. The feeds are whole, minus scripts,
    /// styles and icons, since their posts are read in full; the search ones are reduced to the
    /// elements the parser reads (results, ads and the next-page partial).
    /// </summary>
    [TestClass]
    public class ShredditListingParserTests
    {
        private static string Fixture(string name) => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", name));

        private static List<string> Names(List<Thing<Link>> posts) => posts.Select(p => p.Data!.Name).ToList();

        [TestMethod]
        public void ParseFeed_ReadsPostsInOrderWithoutAds()
        {
            (List<Thing<Link>> posts, string? after) = ShredditListingParser.ParseFeed(Fixture("shreddit-feed-community.html"));

            // /r/LocalLLaMA/new, page 1, as www served it on 2026-11-08 (4 ads among the posts).
            Assert.AreEqual(24, posts.Count);
            CollectionAssert.AreEqual(new[] { "t3_1xr78s5", "t3_1xr777l", "t3_1xr6vw0", "t3_1xr6krp" }, Names(posts).Take(4).ToList());
            CollectionAssert.DoesNotContain(Names(posts), "t3_1veqf5c", "an ad");
            Assert.AreEqual("t3_1xr18t3", after);
        }

        [TestMethod]
        public void ParseFeed_ReadsEachPostInFull()
        {
            (List<Thing<Link>> posts, _) = ShredditListingParser.ParseFeed(Fixture("shreddit-feed-community.html"));

            Thing<Link> thing = posts[0];
            Link post = thing.Data!;

            Assert.AreEqual(ThingKind.Link, thing.Kind);
            Assert.AreEqual("1xr78s5", post.Id);
            Assert.IsTrue(post.Title!.StartsWith("Intel Arc Pro B70 32GB", StringComparison.Ordinal));
            Assert.AreEqual("LocalLLaMA", post.Subreddit);
            Assert.AreEqual(9, post.Score);
            Assert.AreEqual(3, post.NumComments);
            Assert.AreEqual("Showcase", post.LinkFlairText);
            Assert.IsTrue(post.Thumbnail!.StartsWith("https://preview.redd.it/", StringComparison.Ordinal));
        }

        [TestMethod]
        public void ParseFeed_ReadsTheHomeFeed()
        {
            (List<Thing<Link>> posts, string? after) = ShredditListingParser.ParseFeed(Fixture("shreddit-feed-home.html"));

            // Its first page is three posts.
            CollectionAssert.AreEqual(new[] { "t3_1xqt5pi", "t3_1xr3w7e", "t3_1xoz0l3" }, Names(posts));
            CollectionAssert.DoesNotContain(Names(posts), "t3_1veqf64", "an ad");
            Assert.AreEqual("t3_1xoz0l3", after);
        }

        [TestMethod]
        public void ParseFeed_ReadsAnEmptyFeed()
        {
            (List<Thing<Link>> posts, string? after) = ShredditListingParser.ParseFeed(Fixture("shreddit-feed-empty.html"));

            Assert.AreEqual(0, posts.Count);
            Assert.IsNull(after);
        }

        [TestMethod]
        public void ParseSearch_ReadsResultsWithoutAds()
        {
            (List<string> names, string? cursor) = ShredditListingParser.ParseSearch(Fixture("shreddit-search.html"));

            Assert.AreEqual(7, names.Count);
            Assert.AreEqual("t3_1xqq2wf", names[0]);
            CollectionAssert.DoesNotContain(names, "t3_1xpjnn3", "an ad");
            Assert.IsFalse(string.IsNullOrEmpty(cursor));
        }

        [TestMethod]
        public void ParseSearch_ReadsTheRelevanceLayout()
        {
            // Sorted by relevance, www lays results out with content previews instead.
            (List<string> names, string? cursor) = ShredditListingParser.ParseSearch(Fixture("shreddit-search-relevance.html"));

            Assert.AreEqual(7, names.Count);
            Assert.AreEqual("t3_1xpce54", names[0]);
            Assert.IsFalse(string.IsNullOrEmpty(cursor));
        }

        [TestMethod]
        public void EncodeAfter_IsTheBase64OfTheFullname()
        {
            Assert.AreEqual("dDNfMXhxaTRpdQ==", ShredditListingParser.EncodeAfter("t3_1xqi4iu"));
        }
    }
}
