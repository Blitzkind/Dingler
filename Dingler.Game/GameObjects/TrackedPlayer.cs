extern alias HexGame;

using Dingler.Game.GameObjects.TrackedGameZones;
using Dingler.Game.Games;
using HexGame::Game.Shared;

namespace Dingler.Game.GameObjects
{
public sealed class TrackedPlayer : RemotePlayer, IDisposable
{
        private readonly TrackedDeck _trackedDeck;
        private readonly TrackedHand _trackedHand;
        private readonly TrackedVoid _trackedVoid;
        private readonly TrackedDiscard _trackedDiscard;
        private readonly TrackedCastSpells _trackedCastSpells;
        private readonly TrackedChampions _trackedChampions;
        private readonly TrackedPlayedResources _trackedPlayedResources;
        private readonly TrackedSimulacrum _trackedSimulacrum;
        private readonly TrackedUnderground _trackedUnderground;
        private readonly TrackedWarzone _trackedWarzone;
        private readonly TrackedChoosing _trackedChoosing;
        private readonly TrackedNone _trackedNoneZone;
        public readonly GameTimer GameTimer;
        
        public TrackedPlayer()
            : base()
        {
            _trackedDeck = new TrackedDeck();
            _trackedHand = new TrackedHand();
            _trackedVoid = new TrackedVoid();
            _trackedDiscard = new TrackedDiscard();
            _trackedCastSpells = new TrackedCastSpells();
            _trackedChampions = new TrackedChampions();
            _trackedPlayedResources = new TrackedPlayedResources();
            _trackedSimulacrum = new TrackedSimulacrum();
            _trackedUnderground = new TrackedUnderground();
            _trackedWarzone = new TrackedWarzone();
            _trackedChoosing = new TrackedChoosing();
            _trackedNoneZone = new TrackedNone();
            GameTimer = new GameTimer(m_PlayerId);

            m_Deck = _trackedDeck;
            m_Hand = _trackedHand;
            m_Void = _trackedVoid;
            m_Discard = _trackedDiscard;
            m_CastSpells = _trackedCastSpells;
            m_Champions = _trackedChampions;
            m_PlayedResources = _trackedPlayedResources;
            m_Simulacrum = _trackedSimulacrum;
            m_Underground = _trackedUnderground;
            m_Warzone = _trackedWarzone;
            m_Choosing = _trackedChoosing;
            m_None = _trackedNoneZone;

            m_Deck.ControllingPlayer = this;
            m_Hand.ControllingPlayer = this;
            m_Void.ControllingPlayer = this;
            m_Discard.ControllingPlayer = this;
            m_CastSpells.ControllingPlayer = this;
            m_Champions.ControllingPlayer = this;
            m_PlayedResources.ControllingPlayer = this;
            m_Simulacrum.ControllingPlayer = this;
            m_Underground.ControllingPlayer = this;
            m_Warzone.ControllingPlayer = this;
            m_Choosing.ControllingPlayer = this;
            m_None.ControllingPlayer = this;
        }

        public TrackedPlayer(PlayerState playerState, UID connectionId)
            : base(playerState, connectionId)
        {
            _trackedDeck = new TrackedDeck();
            _trackedHand = new TrackedHand();
            _trackedVoid = new TrackedVoid();
            _trackedDiscard = new TrackedDiscard();
            _trackedCastSpells = new TrackedCastSpells();
            _trackedChampions = new TrackedChampions();
            _trackedPlayedResources = new TrackedPlayedResources();
            _trackedSimulacrum = new TrackedSimulacrum();
            _trackedUnderground = new TrackedUnderground();
            _trackedWarzone = new TrackedWarzone();
            _trackedChoosing = new TrackedChoosing();
            _trackedNoneZone = new TrackedNone();
            GameTimer = new GameTimer(m_PlayerId);

            m_Deck = _trackedDeck;
            m_Hand = _trackedHand;
            m_Void = _trackedVoid;
            m_Discard = _trackedDiscard;
            m_CastSpells = _trackedCastSpells;
            m_Champions = _trackedChampions;
            m_PlayedResources = _trackedPlayedResources;
            m_Simulacrum = _trackedSimulacrum;
            m_Underground = _trackedUnderground;
            m_Warzone = _trackedWarzone;
            m_Choosing = _trackedChoosing;
            m_None = _trackedNoneZone;

            m_Deck.ControllingPlayer = this;
            m_Hand.ControllingPlayer = this;
            m_Void.ControllingPlayer = this;
            m_Discard.ControllingPlayer = this;
            m_CastSpells.ControllingPlayer = this;
            m_Champions.ControllingPlayer = this;
            m_PlayedResources.ControllingPlayer = this;
            m_Simulacrum.ControllingPlayer = this;
            m_Underground.ControllingPlayer = this;
            m_Warzone.ControllingPlayer = this;
            m_Choosing.ControllingPlayer = this;
            m_None.ControllingPlayer = this;
        }

        public void Dispose()
        {
            GameTimer.Dispose();
        }
    }
}
