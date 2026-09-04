extern alias HexGame;
using Dingler.Game.Games;
using HexGame::Game.Shared.Domain;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.States;

public class TournamentRoomState
{
	public TournamentInfo TournamentInfo { get; }
	public Dictionary<string, deck_bits> BaseDeckInfo { get; }
	public Dictionary<string, deck_bits> DeckInfo { get; }
	public GameSettings GameSettings { get; }
	
	public int CurrentRound { get; set;}
	
	public int VersionNumber { get; set; }

	public int WaitingRoomVersion { get; set; }

	public TournamentRoomState(TournamentInfo tournamentInfo, GameSettings gameSettings) : 
		this(tournamentInfo, new List<TournamentPlayerInfo>(), gameSettings)
	{ }

	public TournamentRoomState(TournamentInfo tournamentInfo, IEnumerable<TournamentPlayerInfo> playerInfo,
		GameSettings gameSettings)

	{
		TournamentInfo = tournamentInfo;
		BaseDeckInfo = new Dictionary<string, deck_bits>();
		DeckInfo = new Dictionary<string, deck_bits>();
		GameSettings = gameSettings;
	}
}