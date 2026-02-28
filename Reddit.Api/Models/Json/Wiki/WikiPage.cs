using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Wiki
{
    public class WikiAuthorData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Wiki page content.
    /// </summary>
    public class WikiPage
    {
        [JsonPropertyName("content_html")]
        public string? ContentHtml { get; set; }

        [JsonPropertyName("content_md")]
        public string ContentMd { get; set; } = string.Empty;

        [JsonPropertyName("may_revise")]
        public bool MayRevise { get; set; }

        [JsonPropertyName("revision_by")]
        public WikiRevisionAuthor? RevisionBy { get; set; }

        [JsonPropertyName("revision_date")]
        public double? RevisionDate { get; set; }

        [JsonPropertyName("revision_id")]
        public string? RevisionId { get; set; }
    }

    /// <summary>
    /// Response from GET /r/{subreddit}/wiki/{page}.
    /// </summary>
    public class WikiPageResponse
    {
        [JsonPropertyName("data")]
        public WikiPage? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "wikipage";
    }

    /// <summary>
    /// Wiki page settings.
    /// </summary>
    public class WikiPageSettings
    {
        [JsonPropertyName("editors")]
        public List<WikiRevisionAuthor>? Editors { get; set; }

        [JsonPropertyName("listed")]
        public bool Listed { get; set; }

        [JsonPropertyName("permlevel")]
        public int PermLevel { get; set; }
    }

    /// <summary>
    /// Wiki page listing.
    /// </summary>
    public class WikiPagesResponse
    {
        [JsonPropertyName("data")]
        public List<string> Data { get; set; } = [];

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "wikipagelisting";
    }

    /// <summary>
    /// Wiki revision entry.
    /// </summary>
    public class WikiRevision
    {
        [JsonPropertyName("author")]
        public WikiRevisionAuthor? Author { get; set; }

        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("page")]
        public string? Page { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("timestamp")]
        public double Timestamp { get; set; }
    }

    public class WikiRevisionAuthor
    {
        [JsonPropertyName("data")]
        public WikiAuthorData? Data { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = "t2";
    }
}