using Reddit.Api.Client;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.LinksComments;

namespace Reddit.Api.Tests
{
    [TestClass]
    public class RenderedMoreParserTests
    {
        private static MoreChildrenThingData Fixture() => new()
        {
            Id = "t1_19s3i9cbma3qa",
            Parent = "t1_19s3i3tahaezp",
            // Captured from old.reddit.com/api/morechildren (no .json suffix).
            Content = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "morechildren-more.html"))
        };

        [TestMethod]
        public void Parse_LoadMore_ReadsChildrenCountAndIdentity()
        {
            Comment more = RenderedMoreParser.Parse(Fixture());

            Assert.AreEqual("19s3i9cbma3qa", more.Id);
            Assert.AreEqual("t1_19s3i9cbma3qa", more.Name);
            Assert.AreEqual("t1_19s3i3tahaezp", more.ParentId);
            CollectionAssert.AreEqual(new[] { "19s3i9cbma3qa", "19s3iax2l8go1" }, more.Children);
            Assert.AreEqual(4, more.Count);
        }

        [TestMethod]
        public void Parse_ContinueThisThread_HasNoChildren()
        {
            MoreChildrenThingData data = new()
            {
                Id = "t1__",
                Parent = "t1_19s3i319ys95u",
                // Captured from old.reddit.com/api/morechildren (no .json suffix).
                Content = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "morechildren-continue-thread.html"))
            };

            Comment more = RenderedMoreParser.Parse(data);

            // Reddit's JSON gives "continue this thread" placeholders id "_" and no children.
            Assert.AreEqual("_", more.Id);
            Assert.AreEqual("t1__", more.Name);
            Assert.AreEqual("t1_19s3i319ys95u", more.ParentId);
            Assert.AreEqual(0, more.Children!.Count);
            Assert.AreEqual(0, more.Count);
        }

        [TestMethod]
        public void Parse_UnrecognisedMarkup_Throws()
        {
            MoreChildrenThingData data = new()
            {
                Id = "t1_abc",
                Parent = "t1_parent",
                Content = "<div class=\"thing\" data-fullname=\"t1_abc\"><div class=\"entry\"></div></div>"
            };

            Assert.ThrowsException<InvalidOperationException>(() => RenderedMoreParser.Parse(data));
        }

        [TestMethod]
        public void Parse_MismatchedFullname_Throws()
        {
            MoreChildrenThingData data = Fixture();
            data.Id = "t1_other";

            Assert.ThrowsException<InvalidOperationException>(() => RenderedMoreParser.Parse(data));
        }
    }
}
