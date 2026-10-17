using System.Linq.Expressions;
using Dingler.Data.Context;
using Dingler.Data.Entities.GameData;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Repositories
{
    public class FriendRepository
    {
        private readonly IDbContextFactory<GameDataContext> _factory;

        public FriendRepository(IDbContextFactory<GameDataContext> factory)
        {
            _factory = factory;
        }

        public async Task<T?> GetFriendshipAsync<T>(Expression<Func<Friend, T>> selector, ulong userId1, ulong userId2)
        {
            using var context = await _factory.CreateDbContextAsync();

            return await context.Friends
                .Where(f => (f.RequesterId == userId1 && f.RequestedId == userId2) || f.RequesterId == userId2 && f.RequestedId == userId1)
                .Select(selector).FirstOrDefaultAsync();
        }

        public Task<Friend?> GetFriendshipAsync(ulong userId1, ulong userId2)
        {
            return GetFriendshipAsync(f => f, userId1, userId2);
        }

        public async Task<int> TryAcceptFriendRequestAsync(ulong userid, string friendName)
        {
            var context = await _factory.CreateDbContextAsync();

            return await context.Friends
                .Where(f => f.RequestedId == userid && f.Requester.Username == friendName && f.FriendStatusId == (int)Dingler.Data.Enums.FriendStatus.Pending)
                .Include(f => f.Requester)
                .ExecuteUpdateAsync(setter => setter.SetProperty(f => f.FriendStatusId, (int)Data.Enums.FriendStatus.Accepted));
        }

        public async Task<List<T>> GetAllFriendsWithStatusForUserIdAsync<T>(Expression<Func<Friend, T>> selector, ulong userId, Data.Enums.FriendStatus status)
        {
            using var context = await _factory.CreateDbContextAsync();

            return await context.Friends.Where(f => (f.RequestedId == userId || f.RequesterId == userId) && f.FriendStatusId == (int)status)
                .Select(selector).ToListAsync();
        }

        public async Task<List<T>> GetPendingFriendRequestsForUser<T>(Expression<Func<Friend, T>> selector, ulong userId)
        {
            using var context = await _factory.CreateDbContextAsync();

            return await context.Friends.Where(f => f.FriendStatusId == (int)Data.Enums.FriendStatus.Pending && f.RequestedId == userId).Include(f => f.Requester).Select(selector).ToListAsync();
        }

        public Task<List<Friend>> GetAllFriendsWithStatusForUserIdAsync(ulong userId, Data.Enums.FriendStatus status)
        {
            return GetAllFriendsWithStatusForUserIdAsync(f => f, userId, status);
        }

        public async Task AddFriendRequestAsync(Friend friendRequest)
        {
            using var context = await _factory.CreateDbContextAsync();

            context.Friends.Add(friendRequest);

            await context.SaveChangesAsync();
        }

        public async Task UpdateFriendship(Friend friend)
        {
            using var context = await _factory.CreateDbContextAsync();

            context.Friends.Update(friend);

            await context.SaveChangesAsync();
        }
    }
}
