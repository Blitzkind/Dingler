extern alias HexGame;
using HexGame::Game.Shared.Mechanics;

namespace Dingler.Game.GameObjects.TrackedGameZones
{
    internal class TrackedWarzone : Warzone, ITrackedZone
    {
        public bool AddSentinel { get; set; } = true;
        public event Action<Card>? CardAdded;
        public event Action<Card>? CardRemoved;

        protected override void InternalAddCard(Card card, int index)
        {
            AddSentinel = false;
            base.InternalAddCard(card, index);
            AddSentinel = true;
            CardAdded?.Invoke(card);
        }

        protected override Card InternalRemoveCard(int index)
        {
            var card = base.InternalRemoveCard(index);
            CardRemoved?.Invoke(card);
            return card;
        }
    }
}
