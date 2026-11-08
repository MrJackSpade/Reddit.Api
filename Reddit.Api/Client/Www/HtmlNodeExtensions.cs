using HtmlAgilityPack;

namespace Reddit.Api.Client
{
    internal static class HtmlNodeExtensions
    {
        /// <summary>The attribute's value, or null when the element doesn't have it.</summary>
        public static string? Attr(this HtmlNode node, string name)
        {
            return node.Attributes.Contains(name) ? node.GetAttributeValue(name, string.Empty) : null;
        }
    }
}
