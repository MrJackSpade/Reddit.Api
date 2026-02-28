using Reddit.Api.Models.Enums;
using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Json.Common
{
    /// <summary>
    /// Reddit's "Thing" wrapper - all Reddit objects are wrapped in this structure.
    /// The "kind" field indicates the type (t1=comment, t2=account, t3=link, t4=message, t5=subreddit, t6=award).
    /// </summary>
    public class Thing<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("kind")]
        public ThingKind Kind { get; set; }
    }

    /// <summary>
    /// Non-generic Thing for polymorphic deserialization.
    /// </summary>
    public class Thing
    {
        [JsonPropertyName("data")]
        public object? Data { get; set; }

        [JsonPropertyName("kind")]
        public ThingKind Kind { get; set; }
    }
}