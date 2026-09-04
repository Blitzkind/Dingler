extern alias HexGame;
using Dingler.Game.Tournaments;

namespace Dingler.Game.States;

public sealed class WaitingRoomState
{
	public Tournament CurrentTournament { get; set; }
	public WaitingRoomState(Tournament currentTournament)
	{
		CurrentTournament = currentTournament;
	}
}