extern alias HexGame;

using HexGame::Game.Shared.Domain;
using System.Diagnostics.CodeAnalysis;
using Dingler.Server;

namespace Dingler.Game.Extensions
{
    public static class SessionContextExtensions
    {
        private const string DECKS = "decks";
        private const string ACCOUNT_ID = "account_id";
        private const string PROFILE_ID = "profile_id";
        private const string CURRENT_TOURNAMENT_ID = "currentTournamentId";
        public static bool TryAddDeck(this SessionContext sessionContext, deck_bits deck)
        {
            if (!sessionContext.AdditionalData.TryGetValue(DECKS, out object? dictionary) || dictionary is not Dictionary<ulong, deck_bits> deckCollection)
            {
                deckCollection = new Dictionary<ulong, deck_bits>();
                sessionContext.AdditionalData[DECKS] = deckCollection;
            }

            return deckCollection.TryAdd(deck.Id, deck);
        }

        public static void AddOrUpdateDeck(this SessionContext sessionContext, deck_bits deck)
        {
            if (!sessionContext.AdditionalData.TryGetValue(DECKS, out object? dictionary) || dictionary is not Dictionary<ulong, deck_bits> deckCollection)
            {
                deckCollection = new Dictionary<ulong, deck_bits>();
                sessionContext.AdditionalData[DECKS] = deckCollection;
            }

            deckCollection[deck.Id] = deck;
        }

        public static bool TryGetDeck(this SessionContext sessionContext, ulong id, [MaybeNullWhen(false)] out deck_bits deck)
        {
            if (!sessionContext.AdditionalData.TryGetValue(DECKS, out object? dictionary) || dictionary is not Dictionary<ulong, deck_bits> deckCollection || !deckCollection.TryGetValue(id, out deck))
            {
                deck = null;
                return false;
            }

            return true;
        }

        public static bool RemoveDeck(this SessionContext sessionContext, ulong id, [MaybeNullWhen(false)] out deck_bits deck)
        {
            if (!sessionContext.AdditionalData.TryGetValue(DECKS, out object? dictionary) || dictionary is not Dictionary<ulong, deck_bits> deckCollection || !deckCollection.Remove(id, out deck))
            {
                deck = null;
                return false;
            }

            return true;
        }

        public static ulong GetAccountId(this SessionContext sessionContext)
        {
            if (!sessionContext.AdditionalData.TryGetValue(ACCOUNT_ID, out object? boxedAccountProfileId) || boxedAccountProfileId is not ulong accountId)
            {
                return 0;
            }

            return accountId;
        }

        public static void SetAccountId(this SessionContext sessionContext, ulong accountId)
        {
            sessionContext.AdditionalData[ACCOUNT_ID] = accountId;
        }
        
        
        public static ulong GetProfileId(this SessionContext sessionContext)
        {
            if (!sessionContext.AdditionalData.TryGetValue(PROFILE_ID, out object? boxedAccountProfileId) ||
                boxedAccountProfileId is not ulong accountId) 
            {
                return 0;
            }

            return accountId;
        }
        
        public static void SetProfileId(this SessionContext sessionContext, ulong accountId)
        {
            sessionContext.AdditionalData[PROFILE_ID] = accountId;
        }

        public static void SetCurrentTournamentId(this SessionContext sessionContext, ulong tournamentId)
        {
            sessionContext.AdditionalData[CURRENT_TOURNAMENT_ID] = tournamentId;
        }

        public static bool TryGetCurrentTournamentId(this SessionContext sessionContext, out ulong tournamentId)
        {
            if (!sessionContext.AdditionalData.TryGetValue(CURRENT_TOURNAMENT_ID, out var boxedTournamentId) ||
                boxedTournamentId is not ulong tId)
            {
                tournamentId = 0;
                return false;
            }

            tournamentId = tId;
            return true;
        }
    }
}
