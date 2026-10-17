using Dingler.Data.Context;
using Dingler.Data.Entities.GameData;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Repositories
{
    public sealed class AccountRepository
    {
        private readonly IDbContextFactory<GameDataContext> _factory;

        public AccountRepository(IDbContextFactory<GameDataContext> factory)
        {
            _factory = factory;
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            await using var context = await _factory.CreateDbContextAsync().ConfigureAwait(false);

            var account = await context.Accounts.Include(a => a.PlayerProfile).Where(u => u.Email == email).FirstOrDefaultAsync().ConfigureAwait(false);

            if (account == null)
            {
                var nextAccountId = await context.Accounts
                    .Select(a => a.Id)
                    .DefaultIfEmpty()
                    .MaxAsync()
                    .ConfigureAwait(false) + 1;

                account = new Account()
                {
                    Id = nextAccountId,
                    Email = email,
                };

                var playerProfile = new PlayerProfile()
                {
                    Account = account,
                    Username = email,
                    RankId = (int)Dingler.Data.Enums.Rank.Bronze
                };

                account.PlayerProfile = playerProfile;

                await context.Accounts.AddAsync(account).ConfigureAwait(false);

                await context.PlayerProfiles.AddAsync(playerProfile).ConfigureAwait(false);

                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            return account;
        }

        public async Task<ulong> GetIdByUsernameAsync(string username)
        {
            await using var context = await _factory.CreateDbContextAsync().ConfigureAwait(false);

            return Convert.ToUInt64(await context.PlayerProfiles
                .Where(pp => pp.Username.ToLower() == username.ToLower())
                .Select(pp => pp.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false));
        }
    }
}
