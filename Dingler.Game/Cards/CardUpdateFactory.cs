extern alias HexGame;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Mechanics;

namespace Dingler.Game.Cards
{ 
    public static class CardUpdateFactory 
    {
        public static CardUpdatedSessionEventArgs CreateUpdateEventForPlayer(Player player, Card card, ECardCollections zone, bool forceFaceup = false)
        {

            if (zone == ECardCollections.PlayedResources)
            {
                zone = ECardCollections.None;
            }

            CardUpdatedSessionEventArgs updateEvent = card.ConvertToUpdateEventForPlayer(player, forceFaceup || card.CurrentType == ECardTypes.Choice);
            updateEvent.Collection = zone;
            updateEvent.Controller = card.GetControllingPlayer().m_PlayerId;
            updateEvent.SessionCardId = card.m_SessionCardId;
            updateEvent.SessionId = card.m_Session.m_SessionId;

            return updateEvent;
        }
    }
}
