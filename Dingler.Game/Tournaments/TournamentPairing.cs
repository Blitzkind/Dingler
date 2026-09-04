extern alias HexGame;
using HexGame::Game.Shared.Domain;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Tournaments;

public sealed class TournamentPairing
{
	public TournamentPlayerInfo Player1 { get; }
	public TournamentPlayerInfo Player2 { get; }
	public Dictionary<string, deck_bits> Decks { get; }
	public bool Bye { get; }

	public TournamentPairing(TournamentPlayerInfo player1, TournamentPlayerInfo? player2, Dictionary<string, deck_bits> decks, bool bye = false)
	{
		Player1 = player1;
		Player2 = player2!;
		Decks = decks;
		Bye = bye;
	}
}
