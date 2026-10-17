extern alias HexGame;
using System.Text.Json;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Mechanics.Transactions;

namespace Dingler.Game.Games.Transactions;

public class GameStateRequestTransaction : Transaction
{
	public override void Resolve(Session session)
	{
		
	}

	public override string GetLogLine(Session session)
	{
		return "Dummy";
	}
}