using Reddit.Api.Models.Json.Account;

namespace Reddit.Api.Client
{
    public partial class RedditClient
    {
        /// <inheritdoc />
        public async Task<MeResponse?> GetMeAsync(CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await GetAsync<MeResponse>("/api/v1/me", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<KarmaResponse?> GetMyKarmaAsync(CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await GetAsync<KarmaResponse>("/api/v1/me/karma", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PrefsResponse?> GetMyPrefsAsync(CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await GetAsync<PrefsResponse>("/api/v1/me/prefs", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PrefsResponse?> UpdateMyPrefsAsync(PrefsResponse prefs, CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await PatchJsonAsync<PrefsResponse>("/api/v1/me/prefs", prefs, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<TrophyListResponse?> GetMyTrophiesAsync(CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await GetAsync<TrophyListResponse>("/api/v1/me/trophies", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<UserListResponse?> GetMyFriendsAsync(CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await GetAsync<UserListResponse>("/api/v1/me/friends", cancellationToken);
        }

        /// <inheritdoc />
        public async Task<UserListResponse?> GetMyBlockedAsync(CancellationToken cancellationToken = default)
        {
            await EnsureAuthenticatedAsync(cancellationToken);
            return await GetAsync<UserListResponse>("/prefs/blocked", cancellationToken);
        }
    }
}
