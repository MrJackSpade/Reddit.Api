using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using System.Text.Json;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<Thing<Comment>?> CommentAsync(string parentFullname, string text, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["thing_id"] = parentFullname,
                ["text"] = text
            };

            ApiResponse<CommentResponseData>? response = await this.PostFormAsync<ApiResponse<CommentResponseData>>("/api/comment", formData, cancellationToken);
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

            return await this.PostFormAsync("/api/del", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Thing<Comment>?> EditAsync(string fullname, string text, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["thing_id"] = fullname,
                ["text"] = text
            };

            ApiResponse<CommentResponseData>? response = await this.PostFormAsync<ApiResponse<CommentResponseData>>("/api/editusertext", formData, cancellationToken);
            CheckApiResponse(response);

            return response?.Json?.Data?.Things?.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<bool> FollowPostAsync(string fullname, bool follow, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["fullname"] = fullname,
                ["follow"] = follow.ToString().ToLower()
            };

            return await this.PostFormAsync("/api/follow_post", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Listing<Thing<Link>>?> GetInfoAsync(IEnumerable<string>? ids = null, string? url = null, CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            List<string> queryParams = new();
            if (ids != null)
            {
                queryParams.Add($"id={string.Join(",", ids)}");
            }

            if (!string.IsNullOrEmpty(url))
            {
                queryParams.Add($"url={Uri.EscapeDataString(url)}");
            }

            string query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
            return await this.GetAsync<Listing<Thing<Link>>>($"/api/info{query}", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<List<Thing<Comment>>?> GetMoreChildrenAsync(
            string linkFullname,
            IEnumerable<string> children,
            string? sort = null,
            int? depth = null,
            CancellationToken cancellationToken = default)
        {
            await this.TryAuthenticateAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["link_id"] = linkFullname,
                ["children"] = string.Join(",", children),
                ["limit_children"] = "false"
            };

            if (!string.IsNullOrEmpty(sort))
            {
                formData["sort"] = sort;
            }

            if (depth.HasValue)
            {
                formData["depth"] = depth.Value.ToString();
            }

            ApiResponse<MoreChildrenResponseData>? response = await this.PostFormAsync<ApiResponse<MoreChildrenResponseData>>("/api/morechildren", formData, cancellationToken);
            CheckApiResponse(response);

            // Parse the response - it contains things with different kinds
            List<Thing<Comment>> result = new();
            if (response?.Json?.Data?.Things != null)
            {
                foreach (MoreChildrenThing thing in response.Json.Data.Things)
                {
                    if (thing.Kind == ThingKind.Comment && thing.Data != null)
                    {
                        string json = JsonSerializer.Serialize(thing.Data, _jsonOptions);
                        Comment? comment = JsonSerializer.Deserialize<Comment>(json, _jsonOptions);
                        if (comment != null)
                        {
                            result.Add(new Thing<Comment> { Kind = ThingKind.Comment, Data = comment });
                        }
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<bool> HideAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/hide", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> LockAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/lock", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> MarkNsfwAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/marknsfw", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> MarkSpoilerAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/spoiler", formData, cancellationToken);
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

            return await this.PostFormAsync("/api/report", formData, cancellationToken);
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

            return await this.PostFormAsync("/api/save", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> SetContestModeAsync(string fullname, bool state, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["id"] = fullname,
                ["state"] = state.ToString().ToLower()
            };

            return await this.PostFormAsync("/api/set_contest_mode", formData, cancellationToken);
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

            return await this.PostFormAsync("/api/sendreplies", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> SetStickyAsync(string fullname, bool state, int? num = null, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["api_type"] = "json",
                ["id"] = fullname,
                ["state"] = state.ToString().ToLower()
            };

            if (num.HasValue)
            {
                formData["num"] = num.Value.ToString();
            }

            return await this.PostFormAsync("/api/set_subreddit_sticky", formData, cancellationToken);
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

            if (request.Nsfw.HasValue)
            {
                formData["nsfw"] = request.Nsfw.Value.ToString().ToLower();
            }

            if (request.Spoiler.HasValue)
            {
                formData["spoiler"] = request.Spoiler.Value.ToString().ToLower();
            }

            if (request.Resubmit.HasValue)
            {
                formData["resubmit"] = request.Resubmit.Value.ToString().ToLower();
            }

            if (!string.IsNullOrEmpty(request.FlairId))
            {
                formData["flair_id"] = request.FlairId;
            }

            if (!string.IsNullOrEmpty(request.FlairText))
            {
                formData["flair_text"] = request.FlairText;
            }

            ApiResponse<SubmitResponseData>? response = await this.PostFormAsync<ApiResponse<SubmitResponseData>>("/api/submit", formData, cancellationToken);
            CheckApiResponse(response);

            return response?.Json?.Data;
        }

        /// <inheritdoc />
        public async Task<bool> UnhideAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unhide", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> UnlockAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unlock", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> UnmarkNsfwAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unmarknsfw", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> UnmarkSpoilerAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unspoiler", formData, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<bool> UnsaveAsync(string fullname, CancellationToken cancellationToken = default)
        {
            await this.EnsureAuthenticatedAsync(cancellationToken);

            Dictionary<string, string> formData = new()
            {
                ["id"] = fullname
            };

            return await this.PostFormAsync("/api/unsave", formData, cancellationToken);
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

            return await this.PostFormAsync("/api/vote", formData, cancellationToken);
        }
    }

    // Helper class for comment response
    internal class CommentResponseData
    {
        public List<Thing<Comment>>? Things { get; set; }
    }
}