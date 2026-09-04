extern alias HexGame;

using Dingler.Game.Extensions;
using HexGame::Game.Client.Network.Profile;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Domain;
using HexGame::Game.Shared.Mechanics;
using HexGame::Game.Shared.Network.Profile;
using System.Text.Json;
using Dingler.Server;
using Dingler.Data.Repositories;
using Dingler.Game.Domain;
using Deck = Dingler.Data.Entities.GameData.Deck;

namespace Dingler.Game.Services
{
                    public sealed class DeckService
    {
        private readonly DeckRepository _deckRepository;
        private readonly CollectionCacheService _collectionService;

        public DeckService(DeckRepository deckRepository, CollectionCacheService collectionService)
        {
            _deckRepository = deckRepository;
            _collectionService = collectionService;
        }

        public async Task<List<Deck>> GetPlayerDecksAsync(ulong playerProfileId)
        {
            var decks = await _deckRepository.GetAllDecksOwnedByPlayerIdAsync(playerProfileId).ConfigureAwait(false);

            return decks
                .OrderBy(d => d.Id)
                .ToList();
        }

        public async Task<AddNewDeckResponse> AddNewDeck(SessionContext context, AddNewDeckRequestArgs args)
        {
            var playerId = context.GetProfileId();

            var removeTask = _deckRepository.RemoveDeckWithNameOwnedByPlayer(args.DeckName, playerId);

            var newDeck = new Deck()
            {
                ChampionGuid = args.PvPChampionId.m_Guid,
                DeckGuid = Guid.NewGuid(),
                DeckName = args.DeckName,
                PlayerProfileId = playerId,
            };

            var removedDeckId = await removeTask;

            if (removedDeckId != 0)
                context.RemoveDeck(removedDeckId, out var _);

            var addedDeck = await _deckRepository.CreateDeckAsync(newDeck);

            var deckbitsString = CreateDeckBitsStringForNewDeck(addedDeck.Id, args);

            addedDeck.DeckBitsJson = deckbitsString;

            await _deckRepository.UpdateDeckAsync(addedDeck);

            var dinglerDeckBits = JsonSerializer.Deserialize<DinglerDeckBits>(addedDeck.DeckBitsJson);

            var deckBits = dinglerDeckBits?.ToDeckBits();

            if (deckBits is null || !context.TryAddDeck(deckBits))
                throw new Exception($"Could not add deck to user's cached decks");

            var response = new AddNewDeckResponse
            {
                Deckbits = deckBits ?? new deck_bits(),
                DeckID = new UID(UID.Type.Deck, (ulong)addedDeck.Id),
                DeckName = addedDeck.DeckName,
                Error = EAddNewDeckError.Ok,
            };

            return response;
        }

        public GetDeckInfoResponse GetDeckInfo(SessionContext userProfile, GetDeckInfoRequestArgs args)
        {
            if (!userProfile.TryGetDeck(args.DeckID.GetInstanceId(), out var deck))
            {
                throw new InvalidOperationException($"No such deck with id {args.DeckID.GetInstanceId()} available.");
            }

            var deckInfoResponse = new GetDeckInfoResponse()
            {
                CoinId = deck.Coin,
                DeckID = args.DeckID,
                DeckName = deck.DeckName,
                DeckSleeveId = deck.deck_sleeve,
                GameboardId = deck.gameboard,
                Persona = deck.Personality,
                PvEChampionId = new UID(UID.Type.Champion, deck.PVEChampionId),
                PvPChampionId = deck.PVPChampionId,
                ActiveGems = deck.ActiveGems.ToDictionary(),
                EquipmentIDs = new List<ResourceId>
                {
                    deck.equipment_1,
                    deck.equipment_2,
                    deck.equipment_3,
                    deck.equipment_4,
                    deck.equipment_5,
                    deck.equipment_6,
                },
                DeckCardIDs = [.. deck.CardsInDeck.Select(c => c.CardId.InstanceId)],
                SideboardCardIDs = [.. deck.CardsInSideboard.Select(c => c.CardId.InstanceId)],
                TalentIDs = new List<ResourceId>
                {
                    deck.talent_1,
                    deck.talent_2,
                    deck.talent_3,
                    deck.talent_4,
                    deck.talent_5
                }
            };

            return deckInfoResponse;
        }

        public async Task<UpdateDeckResponse> UpdateDeckAsync(SessionContext context, UpdateDeckRequestArgs args)
        {
            var deckId = args.DeckID.GetInstanceId();

            var isUpdate = context.TryGetDeck(deckId, out var deckBits);

            if (!isUpdate)
            {
                deckBits = new deck_bits();
            }

            var deck = await _deckRepository.GetDeckById((int)deckId).ConfigureAwait(false);

            if (deck is null)
            {
                if (isUpdate)
                {
                    throw new InvalidOperationException($"Deck id {deckId} does not exist in database but was found in session for player {context.GetProfileId()}");
                }

                deck = new Deck();
                deck.DeckName = args.DeckName;
                deck.DeckGuid = Guid.NewGuid();
                deck.PlayerProfileId = context.GetProfileId();
                deck.ChampionGuid = ResourceId.Invalid.m_Guid;

                await _deckRepository.CreateDeckAsync(deck).ConfigureAwait(false);
                deckBits!.Id = (ulong)deck.Id;
                context.AddOrUpdateDeck(deckBits);
            }

            deckBits!.ActiveGems = args.ActiveGems;
            deckBits.deck_sleeve = args.DeckSleeveId;
            deckBits.DeckName = args.DeckName;
            deckBits.gameboard = args.GameboardId;
            deckBits.Coin = args.CoinId;
            deckBits.equipment_1 = args.EquipmentIDs.Count >= 1 ? args.EquipmentIDs[0] : ResourceId.Invalid;
            deckBits.equipment_2 = args.EquipmentIDs.Count >= 2 ? args.EquipmentIDs[1] : ResourceId.Invalid;
            deckBits.equipment_3 = args.EquipmentIDs.Count >= 3 ? args.EquipmentIDs[2] : ResourceId.Invalid;
            deckBits.equipment_4 = args.EquipmentIDs.Count >= 4 ? args.EquipmentIDs[3] : ResourceId.Invalid;
            deckBits.equipment_5 = args.EquipmentIDs.Count >= 5 ? args.EquipmentIDs[4] : ResourceId.Invalid;
            deckBits.equipment_6 = args.EquipmentIDs.Count >= 6 ? args.EquipmentIDs[5] : ResourceId.Invalid;
            deckBits.talent_1 = args.TalentIDs.Count >= 1 ? args.TalentIDs[0] : ResourceId.Invalid;
            deckBits.talent_2 = args.TalentIDs.Count >= 2 ? args.TalentIDs[1] : ResourceId.Invalid;
            deckBits.talent_3 = args.TalentIDs.Count >= 3 ? args.TalentIDs[2] : ResourceId.Invalid;
            deckBits.talent_4 = args.TalentIDs.Count >= 4 ? args.TalentIDs[3] : ResourceId.Invalid;
            deckBits.talent_5 = args.TalentIDs.Count >= 5 ? args.TalentIDs[4] : ResourceId.Invalid;
            deckBits.Personality = args.Persona;
            deckBits.PVEChampionId = args.PvEChampionId.GetInstanceId();
            deckBits.PVPChampionId = args.PvPChampionId;

            deckBits.CardsInDeck.Clear();
            foreach (var card in args.DeckCardIDs)
            {
                if (!_collectionService.CollectionIds.TryGetValue(card, out var templateId))
                    continue;

                if (!deckBits.ActiveGems.TryGetValue(card, out var activeGems))
                {
                    activeGems = EGemTypesNew.GemFormatBit;
                }

                deckBits.CardsInDeck.Add(new card_instance_bits()
                {
                    Id = card,
                    TemplateID = templateId,
                    CardStats = null,
                    IsFoil = false,
                    IsExtended = false,
                    SocketedGems = activeGems,
                    IsNotTradeable = true,
                    EscrowStatus = null
                });
            }

            deckBits.CardsInSideboard.Clear();
            foreach (var card in args.SideboardCardIDs)
            {
                if (!_collectionService.CollectionIds.TryGetValue(card, out var templateId))
                    continue;

                if (!deckBits.ActiveGems.TryGetValue(card, out var activeGems))
                {
                    activeGems = EGemTypesNew.GemFormatBit;
                }

                deckBits.CardsInSideboard.Add(new card_instance_bits()
                {
                    Id = card,
                    TemplateID = templateId,
                    CardStats = null,
                    IsFoil = false,
                    IsExtended = false,
                    SocketedGems = activeGems,
                    IsNotTradeable = true,
                    EscrowStatus = null
                });
            }

            var dinglerBits = new DinglerDeckBits(deckBits);

            var deckJson = JsonSerializer.Serialize(dinglerBits);

            deck.DeckBitsJson = deckJson;
            deck.DeckName = args.DeckName;
            deck.ChampionGuid = args.PvPChampionId.m_Guid;

            await _deckRepository.UpdateDeckAsync(deck);

            return new UpdateDeckResponse()
            {
                DeckID = new UID(UID.Type.Deck, (ulong)deck.Id),
                updated = isUpdate,
                Error = EUpdateDeckError.Ok,
            };
        }

        public async Task<RemoveDeckResponse> RemoveDeckAsync(SessionContext userProfile, RemoveDeckRequestArgs args)
        {
            var deckId = args.DeckID.GetInstanceId();
            var removeTask = _deckRepository.RemoveDeckAsync((int)deckId);

            userProfile.RemoveDeck(deckId, out var _);

            await removeTask;

            return new RemoveDeckResponse()
            {
                DeckID = new UID(UID.Type.Deck, 0),
                Error = ERemoveDeckError.Ok,
                succeded = true,
            };
        }

        private string CreateDeckBitsStringForNewDeck(int id, AddNewDeckRequestArgs args)
        {
            var deckBits = new DinglerDeckBits()
            {
                Id = (ulong)id,
                PVEChampionId = args.PvEChampionId.GetInstanceId(),
                PVPChampionId = args.PvPChampionId,
                ActiveGems = args.ActiveGems,
                Coin = args.CoinId,
                DeckName = args.DeckName,
                DeckSleeve = args.DeckSleeveId,
                Personality = args.Persona,
                PlayerId = args.RecID.GetInstanceId(),
                Equipment = args.EquipmentIDs,
                Talents = args.TalentIDs
            };

            foreach (var cardId in args.DeckCardIDs)
            {
                if (!_collectionService.CollectionIds.TryGetValue(cardId, out var collectionId))
                    continue;

                deckBits.CardsInDeck.Add(new DinglerCardBits()
                {
                    Id = cardId,
                    TemplateId = collectionId
                });
            }

            foreach (var cardId in args.SideboardCardIDs)
            {
                if (!_collectionService.CollectionIds.TryGetValue(cardId, out var collectionId))
                    continue;

                deckBits.CardsInSideboard.Add(new DinglerCardBits()
                {
                    Id = cardId,
                    TemplateId = collectionId,
                });
            }

            var deckbitsJson = JsonSerializer.Serialize(deckBits);

            return deckbitsJson;
        }
    }
}
