using Dingler.Data.Context;
using Dingler.Data.Entities.GameData;
using Microsoft.EntityFrameworkCore;

namespace Dingler.Data.Repositories
{
    public sealed class DeckRepository
    {
        private readonly IDbContextFactory<GameDataContext> _factory;

        public DeckRepository(IDbContextFactory<GameDataContext> factory)
        {
            _factory = factory;
        }

        public async Task<Deck?> GetDeckById(int id)
        {
            await using var context = await _factory.CreateDbContextAsync().ConfigureAwait(false);

            return await context.Decks
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Deck>> GetAllDecksOwnedByPlayerIdAsync(ulong id)
        {
            await using var context = await _factory.CreateDbContextAsync()
                .ConfigureAwait(false);

            return await context.Decks
                .Where(d => d.PlayerProfileId == id)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Deck> CreateDeckAsync(Deck deck)
        {
            await using var context = await _factory.CreateDbContextAsync()
                .ConfigureAwait(false);

            context.Decks.Add(deck);
            
            await context.SaveChangesAsync()
                .ConfigureAwait(false);

            return deck;
        }

        public async Task UpdateDeckAsync(Deck deck)
        {
            await using var context = await _factory.CreateDbContextAsync()
                .ConfigureAwait(false);

            context.Decks.Update(deck);

            await context.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task RemoveDeckAsync(int id)
        {
            await using var context = await _factory.CreateDbContextAsync()
                .ConfigureAwait(false);

            var deck = await context.Decks.FindAsync(id);

            if (deck is null)
                return;

            context.Decks.Remove(deck);

            await context.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<ulong> RemoveDeckWithNameOwnedByPlayer(string name, ulong playerId)
        {
            await using var context = await _factory.CreateDbContextAsync().ConfigureAwait(false);

            var deck = await context.Decks.Where(d => d.DeckName.ToLower().Equals(name.ToLower()) && d.PlayerProfileId == playerId).FirstOrDefaultAsync();

            if (deck is null)
            {
                return 0;
            }

            var deckId = (ulong)deck.Id;

            context.Decks.Remove(deck);

            await context.SaveChangesAsync();

            return deckId;
        }
    }
}