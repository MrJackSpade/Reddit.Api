using HtmlAgilityPack;
using Reddit.Api.Client;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Listings;

namespace Reddit.Api.Tests
{
    /// <summary>
    /// Fixtures are a &lt;shreddit-post&gt; (and, from thread pages, its menu, which www renders outside
    /// the post) with scripts, styles and menus removed: thread pages captured in a WebView, and feed
    /// partials for the video and embed. Expected values were checked against Reddit's JSON.
    /// </summary>
    [TestClass]
    public class ShredditPostParserTests
    {
        private static Link Parse(string fixture)
        {
            HtmlDocument document = new();
            document.LoadHtml(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", fixture)));
            return ShredditPostParser.Parse(document.DocumentNode.Descendants("shreddit-post").First());
        }

        [TestMethod]
        public void Parse_ReadsAnImagePost()
        {
            Link post = Parse("shreddit-post-image.html");

            Assert.AreEqual("1xq12b9", post.Id);
            Assert.AreEqual("t3_1xq12b9", post.Name);
            Assert.AreEqual("Gacha Girls Corps New Visual", post.Title);
            Assert.AreEqual("Turbostrider27", post.Author);
            Assert.AreEqual("t2_t27vp", post.AuthorFullname);
            Assert.AreEqual("anime", post.Subreddit);
            Assert.AreEqual("t5_2qh22", post.SubredditId);
            Assert.AreEqual(274, post.Score);
            Assert.AreEqual(39, post.NumComments);
            Assert.AreEqual(DateTimeOffset.Parse("2026-11-07T08:02:31.265+00:00").UtcDateTime, post.CreatedUtc.Value);
            Assert.AreEqual("/r/anime/comments/1xq12b9/gacha_girls_corps_new_visual/", post.Permalink);
            Assert.AreEqual("ogre.natalie.mu", post.Domain);
            Assert.AreEqual("https://ogre.natalie.mu/media/news/comic/2026/1107/gachaanime_main.jpg?imwidth=1460&imdensity=1&impolicy=hq", post.Url);
            Assert.IsFalse(post.IsSelf);
            Assert.AreEqual(VoteState.None, post.Likes);
            Assert.IsTrue(post.Thumbnail!.StartsWith("https://external-preview.redd.it/", StringComparison.Ordinal));

            Assert.AreEqual("Official Media", post.LinkFlairText);
            Assert.AreEqual("#0aa18f", post.LinkFlairBackgroundColor.ToNullableString());
            Assert.AreEqual(FlairTextColor.Light, post.LinkFlairTextColor);
        }

        [TestMethod]
        public void Parse_ReadsAGallery()
        {
            Link post = Parse("shreddit-post-gallery.html");

            Assert.IsTrue(post.IsGallery);
            Assert.IsTrue(post.Over18);
            CollectionAssert.AreEqual(
                new[] { "pjzx31wua50i1", "tku6v56va50i1", "gviyijdva50i1", "uj4xbxlva50i1", "43j28ktva50i1" },
                post.GalleryData!.Items!.Select(i => i.MediaId).ToList());

            MediaMetadata first = post.MediaMetadata!["pjzx31wua50i1"];
            Assert.AreEqual("image/jpeg", first.Mimetype);
            Assert.AreEqual("Image", first.Type);
            Assert.AreEqual("https://i.redd.it/pjzx31wua50i1.jpeg", first.Source!.Url);
            Assert.AreEqual(2667, first.Source.X);
            Assert.AreEqual(4000, first.Source.Y);
        }

        [TestMethod]
        public void Parse_ReadsAVideo()
        {
            Link post = Parse("shreddit-post-video.html");

            Assert.IsTrue(post.IsVideo);
            Assert.AreEqual("https://v.redd.it/5pt7pjrtta0i1", post.Url);
            Assert.AreEqual("https://v.redd.it/5pt7pjrtta0i1/DASHPlaylist.mpd", post.Media!.RedditVideo!.DashUrl);
            Assert.IsTrue(post.Media.RedditVideo.HlsUrl!.StartsWith("https://v.redd.it/5pt7pjrtta0i1/HLSPlaylist.m3u8", StringComparison.Ordinal));
            Assert.AreEqual(6, post.Media.RedditVideo.Duration);
            Assert.IsNotNull(post.Thumbnail);
        }

        [TestMethod]
        public void Parse_ReadsACrosspostsOriginalMedia()
        {
            Link post = Parse("shreddit-post-crosspost.html");

            Assert.AreEqual("https://www.reddit.com/r/mildlyinteresting/comments/1xqhy6e/a_hot_air_balloon_landed_in_my_neighborhood/", post.Url);

            Link original = post.CrosspostParentList!.Single();
            Assert.AreEqual("https://i.redd.it/qac8x4psh90i1.jpeg", original.Url);
            Assert.AreEqual("/r/mildlyinteresting/comments/1xqhy6e/a_hot_air_balloon_landed_in_my_neighborhood/", original.Permalink);
            Assert.AreEqual(original.Thumbnail, post.Thumbnail);
        }

        [TestMethod]
        public void Parse_ReadsATextPostAndItsMenu()
        {
            Link post = Parse("shreddit-post-stickied-text.html");

            Assert.IsTrue(post.IsSelf);
            Assert.IsTrue(post.Stickied);
            Assert.IsFalse(post.Saved);
            Assert.IsTrue(post.SelftextHtml!.Contains("<p>", StringComparison.Ordinal));
            Assert.IsTrue(post.Selftext!.Length > 0);
            Assert.AreEqual(FlairTextColor.Dark, post.LinkFlairTextColor);

            // The page doesn't say the post is distinguished, though Reddit's JSON does.
            Assert.AreEqual(DistinguishedKind.Null, post.Distinguished);
        }

        [TestMethod]
        public void Parse_ReadsAnEmbed()
        {
            Link post = Parse("shreddit-post-embed.html");

            Assert.AreEqual("https://youtu.be/-3mUZiM8rDU?is=X6P7Q-SYj8Cm40OR", post.Url);
            Assert.AreEqual("YouTube", post.Media!.Oembed!.ProviderName);
            Assert.IsTrue(post.Media.Oembed.Html!.StartsWith("<iframe", StringComparison.Ordinal));
        }
    }
}
