extern alias HexGame;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Tournaments;

public class TournamentSettings
{
	public long RegistrationOpenTime { get; }
	public int MaxPlayers { get; }
	public int MinPlayers { get; }
	public string Description { get; }
	public long TournamentStartTime { get; }
	public long TournamentEndTime { get; }
	public ETournamentFormats TournamentFormat { get; }
	public ETournamentStyle TournamentStyle { get; }
	public bool IsSpawner { get; set; }
	public bool IsWaitingRoom { get; set; }

	public TournamentSettings(long registrationOpenTime, int maxPlayers, int minPlayers, string description,
		long tournamentStartTime, long tournamentEndTime, ETournamentFormats tournamentFormat,
		ETournamentStyle tournamentStyle, bool isSpawner, bool isWaitingRoom)
	{
		RegistrationOpenTime = registrationOpenTime;
		MaxPlayers = maxPlayers;
		MinPlayers = minPlayers;
		Description = description;
		TournamentStartTime = tournamentStartTime;
		TournamentEndTime = tournamentEndTime;
		TournamentFormat = tournamentFormat;
		TournamentStyle = tournamentStyle;
		IsSpawner = isSpawner;
		IsWaitingRoom = isWaitingRoom;
	}

	public TournamentSettings CreateWaitingRoomSettings()
	{
		return new TournamentSettings(RegistrationOpenTime, MaxPlayers, MinPlayers, Description, TournamentStartTime,
			TournamentEndTime, TournamentFormat, TournamentStyle, false, true);
	}

}