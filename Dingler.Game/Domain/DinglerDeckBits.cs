extern alias HexGame;
using card_instance_bits = HexGame::Game.Shared.Domain.card_instance_bits;
using deck_bits = HexGame::Game.Shared.Domain.deck_bits;
using EDeckLock = HexGame::Game.Shared.Mechanics.EDeckLock;
using EDeckPersonality = HexGame::Game.Shared.Mechanics.EDeckPersonality;
using EGemTypesNew = HexGame::Game.Shared.Mechanics.EGemTypesNew;
using ResourceId = HexGame::Game.Shared.ResourceId;

namespace Dingler.Game.Domain;

    public sealed class DinglerDeckBits
    {
        public DinglerDeckBits()
        {
            CardsInDeck = new();
            CardsInSideboard = new();
            DeckName = "";
            Talents = new();
            Equipment = new();
            ActiveGems = new();
        }
        public DinglerDeckBits(deck_bits deckBits)
        {
            Id = deckBits.Id;
            DeckName = deckBits.DeckName;
            PVEChampionId = deckBits.PVEChampionId;
            PVPChampionId = deckBits.PVPChampionId;
            Talents = new List<ResourceId>
            {
                deckBits.talent_1,
                deckBits.talent_2,
                deckBits.talent_3,
                deckBits.talent_4,
                deckBits.talent_5,
            };
            Equipment = new List<ResourceId>
            {
                deckBits.equipment_1,
                deckBits.equipment_2,
                deckBits.equipment_3,
                deckBits.equipment_4,
                deckBits.equipment_5,
                deckBits.equipment_6
            };
            CardsInDeck = new List<DinglerCardBits>();
            foreach (var card in deckBits.CardsInDeck)
            {
                CardsInDeck.Add(new DinglerCardBits
                {
                    Id = card.Id,
                    TemplateId = card.TemplateID,
                    CardStats = card.CardStats?.ToDictionary(),
                    IsFoil = card.IsFoil,
                    IsExtended = card.IsExtended,
                    SocketedGems = card.SocketedGems,
                    IsNotTradeable = card.IsNotTradeable,
                    EscrowStatus = card.EscrowStatus,
                });
            }

            CardsInSideboard = new List<DinglerCardBits>();
            foreach (var card in deckBits.CardsInSideboard)
            {
                CardsInSideboard.Add(new DinglerCardBits
                {
                    Id = card.Id,
                    TemplateId = card.TemplateID,
                    CardStats = card.CardStats?.ToDictionary(),
                    IsFoil = card.IsFoil,
                    IsExtended = card.IsExtended,
                    SocketedGems = card.SocketedGems,
                    IsNotTradeable = card.IsNotTradeable,
                    EscrowStatus = card.EscrowStatus,
                });
            }

            ActiveGems = deckBits.ActiveGems.ToDictionary();
            Lock = deckBits.Lock;
            LockHolder = deckBits.LockHolder;
            DeckSleeve = deckBits.deck_sleeve;
            GameBoard = deckBits.gameboard;
            Coin = deckBits.Coin;
            PlayerId = deckBits.player_id;
            Personality = deckBits.Personality;
        }

        public ulong Id { get; set; }
        public string DeckName { get; set; }
        public ulong PVEChampionId { get; set; }
        public ResourceId PVPChampionId { get; set; }
        public List<ResourceId> Talents { get; set; }
        public List<ResourceId> Equipment { get; set; }
        public List<DinglerCardBits> CardsInDeck { get; set; }
        public List<DinglerCardBits> CardsInSideboard { get; set; }
        public Dictionary<ulong, EGemTypesNew> ActiveGems { get; set; }
        public EDeckLock Lock { get; set; }
        public ulong LockHolder { get; set; }
        public ResourceId DeckSleeve { get; set; }
        public ResourceId GameBoard { get; set; }
        public ResourceId Coin { get; set; }
        public ulong PlayerId { get; set; }
        public EDeckPersonality Personality { get; set; }

        public deck_bits ToDeckBits()
        {
            var deckbits = new deck_bits
            {
                Id = Id,
                ActiveGems = ActiveGems,
                Coin = Coin,
                DeckName = DeckName,
                deck_sleeve = DeckSleeve,
                equipment_1 = Equipment[0],
                equipment_2 = Equipment[1],
                equipment_3 = Equipment[2],
                equipment_4 = Equipment[3],
                equipment_5 = Equipment[4],
                equipment_6 = Equipment[5],
                gameboard = GameBoard,
                Lock = Lock,
                LockHolder = LockHolder,
                Personality = Personality,
                player_id = PlayerId,
                PVEChampionId = PVEChampionId,
                PVPChampionId = PVPChampionId,
                talent_1 = Talents.Count >= 1 ? Talents[0] : ResourceId.Invalid,
                talent_2 = Talents.Count >= 2 ? Talents[1] : ResourceId.Invalid,
                talent_3 = Talents.Count >= 3 ? Talents[2] : ResourceId.Invalid,
                talent_4 = Talents.Count >= 4 ? Talents[3] : ResourceId.Invalid,
                talent_5 = Talents.Count >= 5 ? Talents[4] : ResourceId.Invalid,
            };

            foreach (var card in CardsInDeck)
            {
                deckbits.CardsInDeck.Add(new card_instance_bits()
                {
                    Id = card.Id,
                    TemplateID = card.TemplateId,
                    IsFoil = card.IsFoil,
                    IsExtended = card.IsExtended,
                    SocketedGems = card.SocketedGems,
                    IsNotTradeable = card.IsNotTradeable,
                    EscrowStatus = card.EscrowStatus
                });
            }

            foreach (var card in CardsInSideboard)
            {
                deckbits.CardsInSideboard.Add(new card_instance_bits()
                {
                    Id = card.Id,
                    TemplateID = card.TemplateId,
                    IsFoil = card.IsFoil,
                    IsExtended = card.IsExtended,
                    SocketedGems = card.SocketedGems,
                    IsNotTradeable = card.IsNotTradeable,
                    EscrowStatus = card.EscrowStatus
                });

            }
            return deckbits;
        }
    }