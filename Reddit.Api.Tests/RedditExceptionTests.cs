using Reddit.Api.Exceptions;

namespace Reddit.Api.Tests
{
    /// <summary>
    /// Unit tests for the Reddit exception types and the parsing of Reddit's
    /// positional [code, message, field] error arrays. These require no network.
    /// </summary>
    [TestClass]
    public class RedditExceptionTests
    {
        [TestMethod]
        public void RedditApiError_FromArray_ParsesCodeMessageAndField()
        {
            RedditApiError error = RedditApiError.FromArray(["THREAD_LOCKED", "Comments are locked.", "parent"]);

            Assert.AreEqual("THREAD_LOCKED", error.Code);
            Assert.AreEqual("Comments are locked.", error.Message);
            Assert.AreEqual("parent", error.Field);
        }

        [TestMethod]
        public void RedditApiError_FromArray_HandlesMissingParts()
        {
            RedditApiError codeOnly = RedditApiError.FromArray(["RATELIMIT"]);
            Assert.AreEqual("RATELIMIT", codeOnly.Code);
            Assert.IsNull(codeOnly.Message);
            Assert.IsNull(codeOnly.Field);

            RedditApiError empty = RedditApiError.FromArray([]);
            Assert.AreEqual("UNKNOWN", empty.Code);
        }

        [TestMethod]
        public void RedditApiError_ToString_IncludesFieldWhenPresent()
        {
            RedditApiError withField = RedditApiError.FromArray(["THREAD_LOCKED", "Comments are locked.", "parent"]);
            Assert.AreEqual("THREAD_LOCKED: Comments are locked. (parent)", withField.ToString());

            RedditApiError noField = RedditApiError.FromArray(["RATELIMIT", "You are doing that too much."]);
            Assert.AreEqual("RATELIMIT: You are doing that too much.", noField.ToString());

            RedditApiError codeOnly = RedditApiError.FromArray(["SOME_CODE"]);
            Assert.AreEqual("SOME_CODE", codeOnly.ToString());
        }

        [TestMethod]
        public void RedditApiException_ExposesFirstCodeAndAggregatesMessages()
        {
            RedditApiException ex = new(
            [
                RedditApiError.FromArray(["THREAD_LOCKED", "Comments are locked.", "parent"]),
                RedditApiError.FromArray(["RATELIMIT", "Slow down.", null!])
            ]);

            Assert.AreEqual("THREAD_LOCKED", ex.Code);
            StringAssert.Contains(ex.Message, "THREAD_LOCKED");
            StringAssert.Contains(ex.Message, "RATELIMIT");
        }

        [TestMethod]
        public void RedditApiException_HasCode_IsCaseInsensitive()
        {
            RedditApiException ex = new([RedditApiError.FromArray(["THREAD_LOCKED", "Comments are locked.", "parent"])]);

            Assert.IsTrue(ex.HasCode("THREAD_LOCKED"));
            Assert.IsTrue(ex.HasCode("thread_locked"));
            Assert.IsFalse(ex.HasCode("RATELIMIT"));
        }

        [TestMethod]
        public void RedditApiException_IsARedditException()
        {
            RedditApiException ex = new([RedditApiError.FromArray(["THREAD_LOCKED", "Comments are locked.", "parent"])]);
            Assert.IsInstanceOfType<RedditException>(ex);
        }

        [TestMethod]
        public void RedditAuthenticationException_IsARedditException()
        {
            RedditAuthenticationException ex = new("Not logged in.");
            Assert.IsInstanceOfType<RedditException>(ex);
            Assert.AreEqual("Not logged in.", ex.Message);
        }

        [TestMethod]
        public void RedditHttpException_CarriesStatusUrlAndBody()
        {
            RedditHttpException ex = new(System.Net.HttpStatusCode.Forbidden, "https://oauth.reddit.com/api/comment", "forbidden body");

            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, ex.StatusCode);
            Assert.AreEqual("https://oauth.reddit.com/api/comment", ex.Url);
            Assert.AreEqual("forbidden body", ex.ResponseBody);
            Assert.IsInstanceOfType<RedditException>(ex);
            StringAssert.Contains(ex.Message, "403");
            StringAssert.EndsWith(ex.Message, "Reddit responded: forbidden body");
        }

        [TestMethod]
        public void RedditHttpException_MessageUsesHtmlPageTitle()
        {
            RedditHttpException ex = new(System.Net.HttpStatusCode.Forbidden, "https://old.reddit.com/api/x.json",
                "<!doctype html><html><head><title>\n  Blocked &amp; denied\n</title></head><body>...</body></html>");

            StringAssert.EndsWith(ex.Message, "Reddit responded: Blocked & denied");
        }

        [TestMethod]
        public void RedditHttpException_MessageUsesVisibleTextOfUntitledHtml()
        {
            RedditHttpException ex = new(System.Net.HttpStatusCode.Forbidden, "https://old.reddit.com/api/x.json",
                "<body class=\"theme-beta\"><div><style>.a{--x:1rem}</style><script>var x=1;</script><h1>You've been blocked</h1> <p>by network security.</p></div></body>");

            StringAssert.EndsWith(ex.Message, "Reddit responded: You've been blocked by network security.");
        }

        [TestMethod]
        public void RedditHttpException_EmptyBodyAddsNoReason()
        {
            RedditHttpException ex = new(System.Net.HttpStatusCode.Forbidden, "https://old.reddit.com/api/x.json", "");

            StringAssert.EndsWith(ex.Message, "failed with status 403 (Forbidden).");
        }
    }
}
