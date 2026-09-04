extern alias HexGame;
using HexGame::Game.Shared.Mechanics;

namespace Dingler.Game.GameObjects.TrackedGameZones
{
    internal class TrackedCastSpells : CastSpells, ITrackedZone
    {
        public bool AddSentinel { get; set; }

        public event Action<Card>? CardAdded;
        public event Action<Card>? CardRemoved;

        protected override void InternalAddCard(Card card, int index)
        {
            base.InternalAddCard(card, index);
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
