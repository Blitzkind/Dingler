extern alias HexGame;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Games;

public class GameSettings
{
	public ESessionFlags SessionFlags { get; }
	public ETournamentFormats TournamentFormat { get; }
	public SessionStateEncounterData.SeriesType SeriesType { get; }

	public GameSettings(ESessionFlags sessionFlags, ETournamentFormats format,
		SessionStateEncounterData.SeriesType seriesType)
	{
		SessionFlags = sessionFlags;
		TournamentFormat = format;
		SeriesType = seriesType;
	}
}