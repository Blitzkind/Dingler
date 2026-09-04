extern alias HexGame;
using HexGame::Game.Shared.Mechanics;

namespace Dingler.Game.GameObjects.TrackedGameZones
{
    internal interface ITrackedZone
    {
        event Action<Card>? CardAdded;
        event Action<Card>? CardRemoved;
        bool AddSentinel { get; set; }
    }
}
