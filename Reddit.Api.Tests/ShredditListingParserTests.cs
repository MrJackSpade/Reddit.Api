using Reddit.Api.Client;

namespace Reddit.Api.Tests
{
    /// <summary>
    /// Fixtures are www.reddit.com's listing and search partials reduced to the elements the parser
    /// reads (posts, ads, results and the next-page partials), with their identifying attributes.
    /// </summary>
    [TestClass]
    public class ShredditListingParserTests
    {
        private static string Fixture(string name) => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", name));

        [TestMethod]
        public void ParseFeed_ReadsPostsInOrderWithoutAds()
        {
            (List<string> names, string? after) = ShredditListingParser.ParseFeed(Fixture("shreddit-feed-community.html"));

            // /r/pics/new: the same 24 posts, in the same order, as old.reddit.com's first 24.
            Assert.AreEqual(24, names.Count);
            CollectionAssert.AreEqual(new[] { "t3_1xqpnww", "t3_1xqpa27", "t3_1xqoli0", "t3_1xqolea" }, names.Take(4).ToList());
            CollectionAssert.DoesNotContain(names, "t3_1veqqlg", "an ad");
            Assert.AreEqual("t3_1xqi4iu", after);
        }

        [TestMethod]
        public void ParseFeed_ReadsTheHomeFeed()
        {
            (List<string> names, string? after) = ShredditListingParser.ParseFeed(Fixture("shreddit-feed-home.html"));

            // Its first page is three posts.
            Assert.AreEqual(3, names.Count);
            Assert.AreEqual(names[^1], after);
        }

        [TestMethod]
        public void ParseFeed_ReadsAnEmptyFeed()
        {
            (List<string> names, string? after) = ShredditListingParser.ParseFeed(Fixture("shreddit-feed-empty.html"));

            Assert.AreEqual(0, names.Count);
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
