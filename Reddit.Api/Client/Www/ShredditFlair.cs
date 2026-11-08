using HtmlAgilityPack;
using Reddit.Api.Models;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Listings;
using System.Text.RegularExpressions;

namespace Reddit.Api.Client
{
    /// <summary>A flair as Reddit's JSON gives it: its text, richtext parts and colours.</summary>
    public sealed partial record ShredditFlair(string Text, List<FlairRichtext> Richtext, JsonColor Background, FlairTextColor TextColor)
    {
        /// <summary>
        /// Reads a flair www renders as a span whose inline style is the background, whose text class
        /// says light or dark text, holding emoji images (alt "emoji:name:") and text. Null when the
        /// span holds nothing. Comments and posts render flair the same way.
        /// </summary>
        /// <param name="owner">The comment or post it's on, for errors.</param>
        public static ShredditFlair? Parse(HtmlNode span, string owner)
        {
            List<FlairRichtext> parts = [];

            foreach (HtmlNode part in span.Descendants())
            {
                if (part.Name == "img")
                {
                    string alt = part.GetAttributeValue("alt", string.Empty);

                    if (!alt.StartsWith("emoji:", StringComparison.Ordinal))
                    {
                        throw Unrecognised(owner, $"flair image alt '{alt}'");
                    }

                    parts.Add(new FlairRichtext { Type = "emoji", EmojiId = alt["emoji".Length..], Url = HtmlEntity.DeEntitize(part.GetAttributeValue("src", string.Empty)) });
                }
                else if (part.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(part.InnerText))
                {
                    parts.Add(new FlairRichtext { Type = "text", Text = HtmlEntity.DeEntitize(part.InnerText.Trim()) });
                }
            }

            if (parts.Count == 0)
            {
                return null;
            }

            // The parts in order, emoji as their codes. Reddit's own text spaces them as the user typed
            // it, which the markup loses; the app only shows this text with the codes stripped.
            string text = string.Concat(parts.Select(p => p.Type == "emoji" ? p.EmojiId : p.Text));

            Match background = BackgroundColor().Match(span.GetAttributeValue("style", string.Empty));
            HashSet<string> classes = [.. span.GetAttributeValue("class", string.Empty).Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)];

            FlairTextColor textColor = classes.Contains("text-global-white") ? FlairTextColor.Light
                : classes.Contains("text-secondary") ? FlairTextColor.Dark
                : throw Unrecognised(owner, $"flair text colour classes '{span.GetAttributeValue("class", string.Empty)}'");

            JsonColor color = !background.Success ? JsonColor.Empty
                : background.Groups["color"].Value == "transparent" ? JsonColor.FromString("transparent")
                : JsonColor.FromString(background.Groups["color"].Value.ToLowerInvariant());

            return new ShredditFlair(text, parts, color, textColor);
        }

        private static InvalidOperationException Unrecognised(string owner, string problem)
        {
            return new InvalidOperationException($"Unrecognised flair markup from www for {owner}: {problem}.");
        }

        [GeneratedRegex(@"background-color:\s*(?<color>#[0-9A-Fa-f]{3,8}|transparent)")]
        private static partial Regex BackgroundColor();
    }
}
