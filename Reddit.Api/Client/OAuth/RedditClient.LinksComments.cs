using Reddit.Api.Models.Enums;
using Reddit.Api.Models.Json.Common;
using Reddit.Api.Models.Json.LinksComments;
using Reddit.Api.Models.Json.Listings;
using Reddit.Api.Models.Json.Media;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
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

    }
}