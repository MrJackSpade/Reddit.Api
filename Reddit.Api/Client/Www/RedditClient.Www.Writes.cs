using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Media;
using Reddit.Api.Models.Json.Messages;
using Reddit.Api.Models.Json.Moderation;
using Reddit.Api.Models.Json.Multis;
using Reddit.Api.Models.Json.Subreddits;
using Reddit.Api.Models.Json.Users;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Reddit.Api.Client
{
    /// <summary>
    /// The app's writes, sent to www.reddit.com with the session cookies and modhash. See RedditClient.Www.cs for the context.
    /// </summary>
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<Thing<Comment>?> CommentAsync(string parentFullname, RichTextDocument richtext, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["thing_id"] = parentFullname,
                ["richtext_json"] = JsonSerializer.Serialize(richtext, _jsonOptions)
            };

            // When using richtext_json, the response is a raw Comment object
            // rather than the usual {"json":{"data":{"things":[...]}}} wrapper
            Comment? comment = await this.PostFormAsync<Comment>("/api/comment", formData, cancellationToken, RequestRoute.Www);

            if (comment != null)
            {
                return new Thing<Comment>
                {
                    Kind = Models.Enums.ThingKind.Comment,
                    Data = comment
                };
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<Thing<Comment>?> CommentAsync(string parentFullname, string markdown, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["thing_id"] = parentFullname,
                ["text"] = markdown
            };

            ApiResponse<CommentResponseData>? response = await this.PostFormAsync<ApiResponse<CommentResponseData>>("/api/comment", formData, cancellationToken, RequestRoute.Www);
            CheckApiResponse(response);
            return response?.Json?.Data?.Things?.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<bool> DeleteThingAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/del", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<Thing<Comment>?> EditAsync(string fullname, RichTextDocument richtext, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["thing_id"] = fullname,
                ["richtext_json"] = JsonSerializer.Serialize(richtext, _jsonOptions)
            };

            Comment? comment = await this.PostFormAsync<Comment>("/api/editusertext", formData, cancellationToken, RequestRoute.Www);

            if (comment != null)
            {
                return new Thing<Comment>
                {
                    Kind = Models.Enums.ThingKind.Comment,
                    Data = comment
                };
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<Thing<Comment>?> EditAsync(string fullname, string markdown, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["thing_id"] = fullname,
                ["text"] = markdown
            };

            ApiResponse<CommentResponseData>? response = await this.PostFormAsync<ApiResponse<CommentResponseData>>("/api/editusertext", formData, cancellationToken, RequestRoute.Www);
            CheckApiResponse(response);
            return response?.Json?.Data?.Things?.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<bool> HideAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/hide", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> UnhideAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unhide", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> LockAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/lock", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> UnlockAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unlock", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> ReportAsync(
            string fullname,
            string? reason = null,
            string? siteReason = null,
            string? ruleReason = null,
            CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["thing_id"] = fullname
            };

            if (!string.IsNullOrEmpty(reason))
            {
                formData["reason"] = reason;
            }

            if (!string.IsNullOrEmpty(siteReason))
            {
                formData["site_reason"] = siteReason;
            }

            if (!string.IsNullOrEmpty(ruleReason))
            {
                formData["rule_reason"] = ruleReason;
            }

            return await this.PostFormAsync("/api/report", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> SaveAsync(string fullname, string? category = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            if (!string.IsNullOrEmpty(category))
            {
                formData["category"] = category;
            }

            return await this.PostFormAsync("/api/save", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> UnsaveAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unsave", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> SetSendRepliesAsync(string fullname, bool state, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname,
                ["state"] = state.ToString().ToLower()
            };

            return await this.PostFormAsync("/api/sendreplies", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> VoteAsync(string fullname, int direction, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname,
                ["dir"] = direction.ToString()
            };

            return await this.PostFormAsync("/api/vote", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<SubmitResponseData?> SubmitAsync(SubmitRequest request, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["sr"] = request.Subreddit,
                ["title"] = request.Title,
                ["kind"] = request.Kind.ToJsonString(),
                ["sendreplies"] = request.SendReplies.ToString().ToLower()
            };

            if (!string.IsNullOrEmpty(request.Text))
            {
                formData["text"] = request.Text;
            }

            if (!string.IsNullOrEmpty(request.Url))
            {
                formData["url"] = request.Url;
            }

            if (!request.Nsfw.IsNull)
            {
                formData["nsfw"] = request.Nsfw.ToString();
            }

            if (!request.Spoiler.IsNull)
            {
                formData["spoiler"] = request.Spoiler.ToString();
            }

            if (!request.Resubmit.IsNull)
            {
                formData["resubmit"] = request.Resubmit.ToString();
            }

            if (!string.IsNullOrEmpty(request.FlairId))
            {
                formData["flair_id"] = request.FlairId;
            }

            if (!string.IsNullOrEmpty(request.FlairText))
            {
                formData["flair_text"] = request.FlairText;
            }

            ApiResponse<SubmitResponseData>? response = await this.PostFormAsync<ApiResponse<SubmitResponseData>>("/api/submit", formData, cancellationToken, RequestRoute.Www);
            CheckApiResponse(response);

            return response?.Json?.Data;
        }

        /// <inheritdoc />
        public async Task<Thing<Comment>?> DistinguishAsync(string fullname, DistinguishHow how = DistinguishHow.Yes, bool? sticky = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["id"] = fullname,
                ["how"] = how.ToJsonString()
            };

            if (sticky.HasValue)
            {
                formData["sticky"] = sticky.Value.ToString().ToLower();
            }

            ApiResponse<CommentResponseData>? response = await this.PostFormAsync<ApiResponse<CommentResponseData>>("/api/distinguish", formData, cancellationToken, RequestRoute.Www);
            CheckApiResponse(response);

            return response?.Json?.Data?.Things?.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<bool> ReadMessageAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/read_message", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> SubscribeAsync(string subreddit, bool subscribe, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["action"] = subscribe ? SubscribeAction.Subscribe.ToJsonString() : SubscribeAction.Unsubscribe.ToJsonString(),
                ["sr_name"] = subreddit
            };

            return await this.PostFormAsync("/api/subscribe", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> AddSubredditToMultiAsync(string multipath, string subreddit, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            var model = new { name = subreddit };
            Dictionary<string, string> formData = new()
            {
                ["model"] = JsonSerializer.Serialize(model, _jsonOptions)
            };

            return await this.PutFormAsync($"/api/multi/{multipath}/r/{subreddit}", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<MultiResponse?> CreateOrUpdateMultiAsync(string multipath, MultiCreateRequest request, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            // Reddit expects the model as a JSON string in a form parameter
            var model = new
            {
                display_name = request.DisplayName,
                description_md = request.DescriptionMd,
                visibility = request.Visibility,
                key_color = request.KeyColor,
                icon_name = request.IconName,
                weighting_scheme = request.WeightingScheme,
                subreddits = request.Subreddits?.Select(s => new { name = s.Name }).ToList()
            };

            Dictionary<string, string> formData = new()
            {
                ["model"] = JsonSerializer.Serialize(model, _jsonOptions)
            };

            return await this.PutFormAsync<MultiResponse>($"/api/multi/{multipath}", formData, cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteMultiAsync(string multipath, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.DeleteAsync($"/api/multi/{multipath}", cancellationToken, RequestRoute.Www);
        }

        /// <inheritdoc />
        public async Task<bool> RemoveSubredditFromMultiAsync(string multipath, string subreddit, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);
            return await this.DeleteAsync($"/api/multi/{multipath}/r/{subreddit}", cancellationToken, RequestRoute.Www);
        }
    }

    // Helper response class
    internal class SubredditAutocompleteResponse
    {
        [JsonPropertyName("subreddits")]
        public List<SubredditAutocompleteItem>? Subreddits { get; set; }
    }

    // Response envelope for endpoints that return things inside the ApiResponse wrapper
    // (e.g. /api/comment, /api/editusertext, /api/distinguish when called with text=).
    // Reddit returns { json: { errors, data: { things: [{kind:"t1", data:{...}}] } } }.
    internal class CommentResponseData
    {
        [JsonPropertyName("things")]
        public List<Thing<Comment>>? Things { get; set; }
    }
}
