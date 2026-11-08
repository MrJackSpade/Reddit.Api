using HtmlAgilityPack;
using Reddit.Api.Models;
using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Listings;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Reddit.Api.Client
{
    /// <summary>
    /// Reads a post www.reddit.com renders as a &lt;shreddit-post&gt; (a thread page's post) into the
    /// same <see cref="Link"/> shape Reddit's JSON uses. Throws on markup it does not recognise
    /// rather than guessing.
    ///
    /// Where each field comes from:
    /// - &lt;shreddit-post&gt;'s attributes: id, title, author, subreddit, score, comment count, time,
    ///   permalink, domain, the content link, its type (text, image, gallery, video, link, crosspost,
    ///   multi_media), nsfw, spoiler, archived, locked, and the user's vote (vote-type).
    /// - Its &lt;shreddit-post-overflow-menu&gt;: saved, stickied, reply notifications.
    /// - Its &lt;shreddit-post-flair&gt;: the link flair, rendered like comment flair.
    /// - Its &lt;shreddit-post-text-body&gt;: the body, as www renders it (no markdown).
    /// - Media, read by <see cref="ReadMedia"/>: images, gallery items, and Reddit videos.
    ///
    /// Not in the markup: distinguished (the app shows the post without the mark), the body's
    /// markdown, and a crosspost original's author and score (its media, title and link are there).
    /// A Reddit video has no signed DASH URL here; the unsigned one
    /// (v.redd.it/{id}/DASHPlaylist.mpd) answers.
    /// </summary>
    public static partial class ShredditPostParser
    {
        public static Link Parse(HtmlNode post)
        {
            string name = post.Attr("id") ?? throw Unrecognised("a post", "no id");

            if (!name.StartsWith("t3_", StringComparison.Ordinal))
            {
                throw Unrecognised(name, "an id that isn't a post's");
            }

            // On a thread page the post's menu is rendered outside the post; in feeds, inside it.
            HtmlNode? menu = post.OwnerDocument.DocumentNode.Descendants("shreddit-post-overflow-menu").FirstOrDefault(m => m.Attr("post-id") == name);
            HtmlNode? flair = Own(post, "shreddit-post-flair")?.Descendants("span").FirstOrDefault();
            HtmlNode? body = Own(post, "shreddit-post-text-body");

            string postType = Required(post, "post-type", name);
            string domain = Required(post, "domain", name);
            ShredditFlair? linkFlair = flair == null ? null : ShredditFlair.Parse(flair, name);

            Link link = new()
            {
                Id = name[3..],
                Name = name,
                Title = HtmlEntity.DeEntitize(Required(post, "post-title", name)),
                Author = HtmlEntity.DeEntitize(Required(post, "author", name)),
                AuthorFullname = post.Attr("author-id"),
                Subreddit = Required(post, "subreddit-name", name),
                SubredditId = post.Attr("subreddit-id"),
                SubredditNamePrefixed = post.Attr("subreddit-prefixed-name"),
                Score = int.Parse(Required(post, "score", name), CultureInfo.InvariantCulture),
                NumComments = int.Parse(Required(post, "comment-count", name), CultureInfo.InvariantCulture),
                UpvoteRatio = post.Attr("upvote-ratio") is string ratio ? double.Parse(ratio, CultureInfo.InvariantCulture) : 0,
                CreatedUtc = ShredditTime.Parse(Required(post, "created-timestamp", name)) ?? throw Unrecognised(name, "a created time that isn't one"),
                Permalink = Required(post, "permalink", name),
                Domain = domain,
                Url = Absolute(HtmlEntity.DeEntitize(Required(post, "content-href", name))),

                IsSelf = domain.StartsWith("self.", StringComparison.Ordinal),
                IsVideo = postType == "video",
                IsGallery = postType == "gallery" ? JsonBool.True : JsonBool.False,
                Over18 = post.Attributes.Contains("nsfw"),
                Spoiler = post.Attributes.Contains("spoiler"),
                Archived = post.Attributes.Contains("archived"),
                Locked = post.Attributes.Contains("locked"),
                Likes = post.Attr("vote-type") switch
                {
                    null => VoteState.None,
                    "upvote" => VoteState.Upvote,
                    "downvote" => VoteState.Downvote,
                    string other => throw Unrecognised(name, $"vote type '{other}'")
                },

                Saved = menu?.Attributes.Contains("is-post-saved") == true,
                Stickied = menu?.Attributes.Contains("is-post-stickied") == true,
                SendReplies = menu?.Attributes.Contains("is-receiving-post-replies") == true,

                LinkFlairText = linkFlair?.Text,
                LinkFlairRichtext = linkFlair?.Richtext,
                LinkFlairBackgroundColor = linkFlair?.Background ?? JsonColor.Null,
                LinkFlairTextColor = linkFlair?.TextColor ?? FlairTextColor.Null,

                SelftextHtml = body == null ? null : BodyHtml(body, name),
                Selftext = body == null ? string.Empty : Text(body, name)
            };

            if (postType == "crosspost")
            {
                // The original is rendered inside the crosspost: its media, and its permalink as the content link.
                Link original = new()
                {
                    Permalink = new Uri(link.Url!).AbsolutePath,
                    Url = link.Url,
                    Title = link.Title
                };

                ReadMedia(post, original);
                link.CrosspostParentList = [original];

                // Reddit's JSON gives a crosspost the original's preview too.
                link.Preview = original.Preview;
                link.Thumbnail = original.Thumbnail;
            }
            else
            {
                ReadMedia(post, link);
            }

            return link;
        }

        /// <summary>
        /// The post's media into <paramref name="link"/>, as Reddit's JSON gives it:
        /// - A &lt;shreddit-player&gt; is a Reddit video: its HLS playlist (src), from which the video's
        ///   id gives the DASH playlist; its poster is the preview image.
        /// - A &lt;gallery-carousel&gt; is a gallery: each item's original image (i.redd.it/{media id}.{ext})
        ///   and its size, from the preview image beside it.
        /// - Otherwise the post's primary image (data-post-media-primary) is its preview, and the
        ///   original (i.redd.it) the post links to.
        /// - A &lt;shreddit-embed&gt; (YouTube and the like) is the embed's HTML and provider. Its
        ///   thumbnail isn't in the markup.
        /// - A text post's first image, or a link post's thumbnail image, is its thumbnail.
        /// </summary>
        private static void ReadMedia(HtmlNode post, Link link)
        {
            HtmlNode? player = post.Descendants("shreddit-player").FirstOrDefault();
            HtmlNode? gallery = post.Descendants("gallery-carousel").FirstOrDefault();

            if (player != null)
            {
                string hls = HtmlEntity.DeEntitize(player.Attr("src") ?? throw Unrecognised(link.Name, "a video player without a src"));
                Match video = RedditVideoUrl().Match(hls);

                if (!video.Success)
                {
                    throw Unrecognised(link.Name, $"a video player whose src isn't a Reddit video: {hls}");
                }

                (int? width, int? height, int? duration) = PackagedSize(player, link.Name);

                RedditVideo redditVideo = new()
                {
                    DashUrl = $"https://v.redd.it/{video.Groups["id"].Value}/DASHPlaylist.mpd",
                    HlsUrl = hls,
                    Width = width,
                    Height = height,
                    Duration = duration
                };

                link.IsVideo = true;
                link.Media = new Media { Type = "v.redd.it", RedditVideo = redditVideo };
                link.SecureMedia = link.Media;
                link.Url = $"https://v.redd.it/{video.Groups["id"].Value}";

                if (player.Attr("poster") is string poster)
                {
                    link.Preview = PreviewOf(HtmlEntity.DeEntitize(poster), 0, 0);
                    link.Thumbnail = link.Preview.Images![0].Source!.SourceUrl;
                }

                return;
            }

            if (gallery != null)
            {
                link.GalleryData = new GalleryData { Items = [] };
                link.MediaMetadata = [];

                foreach (HtmlNode item in gallery.Descendants("li"))
                {
                    HtmlNode original = item.Descendants("img").FirstOrDefault(img => (img.Attr("src") ?? string.Empty).StartsWith("https://i.redd.it/", StringComparison.Ordinal))
                        ?? throw Unrecognised(link.Name, "a gallery item without its original image");
                    HtmlNode sized = item.Descendants("img").FirstOrDefault(img => img.Attr("width") != null && img.Attr("height") != null)
                        ?? throw Unrecognised(link.Name, "a gallery item without its size");

                    string url = HtmlEntity.DeEntitize(original.Attr("src")!);
                    Match file = RedditImageUrl().Match(url);

                    if (!file.Success)
                    {
                        throw Unrecognised(link.Name, $"a gallery image that isn't i.redd.it/{{id}}.{{ext}}: {url}");
                    }

                    string mediaId = file.Groups["id"].Value;
                    int width = int.Parse(sized.Attr("width")!, CultureInfo.InvariantCulture);
                    int height = int.Parse(sized.Attr("height")!, CultureInfo.InvariantCulture);

                    link.GalleryData.Items.Add(new GalleryItem { MediaId = mediaId });
                    link.MediaMetadata[mediaId] = new MediaMetadata
                    {
                        Id = mediaId,
                        Status = "valid",
                        Type = "Image",
                        Mimetype = $"image/{file.Groups["ext"].Value}",
                        Source = new ImageSource { Url = url, X = width, Y = height }
                    };
                }

                if (link.GalleryData.Items.Count == 0)
                {
                    throw Unrecognised(link.Name, "a gallery without items");
                }

                HtmlNode first = gallery.Descendants("img").First(img => img.Attr("src") != null && img.Attr("width") != null);
                link.Preview = PreviewOf(HtmlEntity.DeEntitize(first.Attr("src")!), int.Parse(first.Attr("width")!, CultureInfo.InvariantCulture), int.Parse(first.Attr("height")!, CultureInfo.InvariantCulture));
                link.Thumbnail = link.Preview.Images![0].Source!.SourceUrl;
                return;
            }

            HtmlNode? primary = post.Descendants("img").FirstOrDefault(img => img.Attributes.Contains("data-post-media-primary"));

            if (primary != null)
            {
                int width = primary.Attr("width") is string w ? int.Parse(w, CultureInfo.InvariantCulture) : 0;
                int height = primary.Attr("height") is string h ? int.Parse(h, CultureInfo.InvariantCulture) : 0;

                link.Preview = PreviewOf(HtmlEntity.DeEntitize(primary.Attr("src") ?? throw Unrecognised(link.Name, "a primary image without a src")), width, height);
                link.Thumbnail = link.Preview.Images![0].Source!.SourceUrl;

                // The original, which an image post links to.
                HtmlNode? original = post.Descendants("img").FirstOrDefault(img => (img.Attr("src") ?? string.Empty).StartsWith("https://i.redd.it/", StringComparison.Ordinal));

                if (original != null && link.Url?.StartsWith("https://i.redd.it/", StringComparison.Ordinal) != true && link.Permalink != null && link.Url?.Contains(link.Permalink, StringComparison.Ordinal) == true)
                {
                    link.Url = HtmlEntity.DeEntitize(original.Attr("src")!);
                }

                return;
            }

            HtmlNode? embed = post.Descendants("shreddit-embed").FirstOrDefault();

            if (embed != null)
            {
                OEmbed oembed = new()
                {
                    Type = "video",
                    Html = HtmlEntity.DeEntitize(embed.Attr("html") ?? throw Unrecognised(link.Name, "an embed without html")),
                    ProviderName = embed.Attr("providername")
                };

                link.Media = new Media { Type = link.Domain, Oembed = oembed };
                link.SecureMedia = link.Media;
            }

            // A text post's first image, as Reddit's JSON takes for its thumbnail.
            HtmlNode? bodyImage = Own(post, "shreddit-post-text-body")?.Descendants("img").FirstOrDefault(img => img.Attr("src") != null);

            if (bodyImage != null)
            {
                link.Thumbnail = HtmlEntity.DeEntitize(bodyImage.Attr("src")!);
                return;
            }

            HtmlNode? thumbnail = post.Descendants("img").FirstOrDefault(img => (img.Attr("src") ?? string.Empty).Contains("external-preview.redd.it/", StringComparison.Ordinal)
                || (img.Attr("src") ?? string.Empty).Contains("b.thumbs.redditmedia.com/", StringComparison.Ordinal));

            if (thumbnail != null)
            {
                link.Thumbnail = HtmlEntity.DeEntitize(thumbnail.Attr("src")!);
            }
        }

        /// <summary>The video's size and length from the player's packaged-media-json, when it has one.</summary>
        private static (int? Width, int? Height, int? Duration) PackagedSize(HtmlNode player, string name)
        {
            if (player.Attr("packaged-media-json") is not string json)
            {
                return (null, null, null);
            }

            using JsonDocument document = JsonDocument.Parse(HtmlEntity.DeEntitize(json));

            if (!document.RootElement.TryGetProperty("playbackMp4s", out JsonElement mp4s))
            {
                throw Unrecognised(name, "packaged-media-json without playbackMp4s");
            }

            int? duration = mp4s.TryGetProperty("duration", out JsonElement d) ? d.GetInt32() : null;

            JsonElement largest = mp4s.GetProperty("permutations").EnumerateArray()
                .Select(p => p.GetProperty("source").GetProperty("dimensions"))
                .MaxBy(dimensions => dimensions.GetProperty("height").GetInt32());

            return (largest.GetProperty("width").GetInt32(), largest.GetProperty("height").GetInt32(), duration);
        }

        private static Preview PreviewOf(string url, int width, int height)
        {
            return new Preview
            {
                Enabled = true,
                Images = [new PreviewImage { Source = new ImageSource { SourceUrl = url, Width = width, Height = height }, Resolutions = [] }]
            };
        }

        /// <summary>The body as www renders it, the inner content of "{id}-post-rtjson-content".</summary>
        private static string BodyHtml(HtmlNode body, string name)
        {
            HtmlNode content = body.Descendants().FirstOrDefault(n => n.Id == $"{name}-post-rtjson-content")
                ?? throw Unrecognised(name, "a text body without its content");

            return content.InnerHtml;
        }

        /// <summary>The body's text, a line per paragraph, with HTML entities decoded.</summary>
        private static string Text(HtmlNode body, string name)
        {
            HtmlNode content = body.Descendants().First(n => n.Id == $"{name}-post-rtjson-content");

            return string.Join("\n", HtmlEntity.DeEntitize(content.InnerText).Split('\n').Select(l => l.Trim()).Where(l => l.Length > 0));
        }

        /// <summary>The post's own element named <paramref name="tag"/>, not one in a post rendered inside it.</summary>
        private static HtmlNode? Own(HtmlNode post, string tag)
        {
            return post.Descendants(tag).FirstOrDefault(node => node.Ancestors("shreddit-post").First() == post);
        }

        private static string Absolute(string url)
        {
            return url.StartsWith('/') ? $"https://www.reddit.com{url}" : url;
        }

        private static string Required(HtmlNode node, string attribute, string name)
        {
            return node.Attr(attribute) ?? throw Unrecognised(name, $"no {attribute} attribute");
        }

        private static InvalidOperationException Unrecognised(string name, string problem)
        {
            return new InvalidOperationException($"Unrecognised post markup from www for {name}: {problem}.");
        }

        [GeneratedRegex(@"^https://v\.redd\.it/(?<id>[A-Za-z0-9]+)/")]
        private static partial Regex RedditVideoUrl();

        [GeneratedRegex(@"^https://i\.redd\.it/(?<id>[A-Za-z0-9]+)\.(?<ext>[a-z0-9]+)$")]
        private static partial Regex RedditImageUrl();
    }
}
