extern alias HexGame;

using HexGame::Game.Shared.Mechanics;

namespace Dingler.Game.Cards
{
public static class CardSnapshotFactory
{
        public static CardSnapshot Create(Card card)
        {
            var hash = HashCard(card);
            return new CardSnapshot(card.m_SessionCardId, hash);
        }

        public static int HashCard(Card card)
        {
            var context = card.GetCardContext();
            var prime = 17;

            var hash = HashCode.Combine(prime, context.m_CurrentType, context.m_CurrentSubtype, context.m_CurrentAttackValue, context.m_CurrentDefenseValue, context.m_CurrentResourceCost, context.m_CastingCostAdjustment, context.m_CurrentAttributeFlags);

            foreach (var threshold in card.Thresholds)
            {
                hash = HashCode.Combine(hash, threshold.GetHashCode());
            }

            hash = HashCode.Combine(hash, card.CurrentCardState, card.CurrentDamageValue, card.EscalationCount);

            hash = HashAllTACs(context, hash);
            var abilities = card.CurrentAbilities.ToList();
            abilities.Sort();
            foreach (var abilityId in abilities)
            {
                hash = HashCode.Combine(hash, abilityId.GetHashCode());
            }
            return hash;
        }

        private static int HashAllTACs(CardContext context, int hash)
        {
            var intAttributes = context.GetCurrentIntAttrs()
                .Where(a => a.Key.NotifyClient && a.Key != IntAttrs.TotalResources && a.Key != IntAttrs.CurrentResources)
                .OrderBy(a => a.Key.name)
                .Select(a => new { a.Key.name, a.Value })
                .ToList();
            
            foreach (var attr in intAttributes)
            {
                var oldHash = hash;
                hash = HashCode.Combine(hash, attr.name, attr.Value);
            }

            var stringAttributes = context.GetCurrentStringAttrs().Where(a => a.Key.NotifyClient).OrderBy(a => a.Key.name).Select(a => new { a.Key.name, a.Value }).ToList();

            
            foreach (var attr in stringAttributes)
            {
                var oldHash = hash;
                hash = HashCode.Combine(hash, attr.name, attr.Value);
            }

            return hash;
        }
    }
}
