using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Common
{
    /// <summary>
    /// Reddit's "Thing" wrapper - all Reddit objects are wrapped in this structure.
    /// The "kind" field indicates the type (t1=comment, t2=account, t3=link, t4=message, t5=subreddit, t6=award).
    /// </summary>
    public class Thing<T>
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

    /// <summary>
    /// Non-generic Thing for polymorphic deserialization.
    /// </summary>
    public class Thing
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public object? Data { get; set; }
    }

    /// <summary>
    /// Thing kind constants matching Reddit's API.
    /// </summary>
    public static class ThingKind
    {
        public const string Comment = "t1";
        public const string Account = "t2";
        public const string Link = "t3";
        public const string Message = "t4";
        public const string Subreddit = "t5";
        public const string Award = "t6";
        public const string Listing = "Listing";
        public const string More = "more";
    }
}
