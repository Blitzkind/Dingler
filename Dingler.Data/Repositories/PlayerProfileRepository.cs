using System.Linq.Expressions;
using Dingler.Data.Context;
using Dingler.Data.Entities.GameData;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Repositories
{
    public sealed class PlayerProfileRepository
    {
        private readonly IDbContextFactory<GameDataContext> _factory;

        public PlayerProfileRepository(IDbContextFactory<GameDataContext> factory)
        {
            _factory = factory;
        }

        public async Task<PlayerProfile?> GetProfileByIdAsync(ulong id)
        {
            using var context = await _factory.CreateDbContextAsync()
                .ConfigureAwait(false);

            return await context.PlayerProfiles
                .Where(pp => pp.Id == id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<PlayerProfile> AddNewProfileAsync(PlayerProfile profile)
        {
            using var context = await _factory.CreateDbContextAsync()
                .ConfigureAwait(false);

            context.PlayerProfiles.Add(profile);

            await context.SaveChangesAsync()
                .ConfigureAwait(false);

            return profile;
        }

        public async Task RemovePlayerProfileAsync(int id)
        {
            using var context = await _factory.CreateDbContextAsync()
                .ConfigureAwait(false);

            var profile = await context.PlayerProfiles.FindAsync(id)
                .ConfigureAwait(false);

            if (profile is null)
                return;

            context.PlayerProfiles.Remove(profile);

            await context.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task UpdatePlayerProfileAsync(PlayerProfile profile)
        {
            using var context = await _factory.CreateDbContextAsync();

            context.PlayerProfiles.Update(profile);

            await context.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<PlayerProfile?> GetProfileByUsername(string username)
        {
            using var context = await _factory.CreateDbContextAsync();

            return await context.PlayerProfiles
                .Where(pp => pp.Username.Equals(username))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<T?> GetProfileByUsernameAsync<T>(Expression<Func<PlayerProfile, T>> selector, string username)
        {
            using var context = await _factory.CreateDbContextAsync();

            return await context.PlayerProfiles
                .Where(pp => pp.Username.ToLower().Equals(username.ToLower()))
                .Select(selector)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public Task<PlayerProfile?> GetProfileByUsernameAsync(string username)
        {
            return GetProfileByUsernameAsync(pp => pp, username);
        }
    }
}
