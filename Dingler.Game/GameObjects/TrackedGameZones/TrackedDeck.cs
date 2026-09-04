extern alias HexGame;

using HexGame::Game.Shared.Mechanics;

namespace Dingler.Game.GameObjects.TrackedGameZones
{
    internal class TrackedDeck : Deck, ITrackedZone
    {
        public bool AddSentinel { get; set; } = true;
        public event Action<Card>? CardAdded;
        public event Action<Card>? CardRemoved;
        public event Action<Card>? TopCardOfDeckChanged;

        protected override void InternalAddCard(Card card, int index)
        {
            base.InternalAddCard(card, index);
            CardAdded?.Invoke(card);

            if (index == 0)
                TopCardOfDeckChanged?.Invoke(card);

        }

        protected override Card InternalRemoveCard(int index)
        {
            var card = base.InternalRemoveCard(index);
            CardRemoved?.Invoke(card);

            if (index == 0)
                TopCardOfDeckChanged?.Invoke(card);

            return card;
        }
    }
}
