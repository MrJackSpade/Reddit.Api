using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Wiki
{
    /// <summary>
    /// Response from GET /r/{subreddit}/wiki/{page}.
    /// </summary>
    public class WikiPageResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "wikipage";

        [JsonPropertyName("data")]
        public WikiPage? Data { get; set; }
    }

    /// <summary>
    /// Wiki page content.
    /// </summary>
    public class WikiPage
    {
        [JsonPropertyName("content_md")]
        public string ContentMd { get; set; } = string.Empty;

        [JsonPropertyName("content_html")]
        public string? ContentHtml { get; set; }

        [JsonPropertyName("revision_id")]
        public string? RevisionId { get; set; }

        [JsonPropertyName("revision_date")]
        public double? RevisionDate { get; set; }

        [JsonPropertyName("revision_by")]
        public WikiRevisionAuthor? RevisionBy { get; set; }

        [JsonPropertyName("may_revise")]
        public bool MayRevise { get; set; }
    }

    public class WikiRevisionAuthor
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "t2";

        [JsonPropertyName("data")]
        public WikiAuthorData? Data { get; set; }
    }

    public class WikiAuthorData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Wiki page listing.
    /// </summary>
    public class WikiPagesResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "wikipagelisting";

        [JsonPropertyName("data")]
        public List<string> Data { get; set; } = [];
    }

    /// <summary>
    /// Wiki page settings.
    /// </summary>
    public class WikiPageSettings
    {
        [JsonPropertyName("permlevel")]
        public int PermLevel { get; set; }

        [JsonPropertyName("editors")]
        public List<WikiRevisionAuthor>? Editors { get; set; }

        [JsonPropertyName("listed")]
        public bool Listed { get; set; }
    }

    /// <summary>
    /// Wiki revision entry.
    /// </summary>
    public class WikiRevision
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        public double Timestamp { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("author")]
        public WikiRevisionAuthor? Author { get; set; }

        [JsonPropertyName("page")]
        public string? Page { get; set; }

        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; }
    }
}
