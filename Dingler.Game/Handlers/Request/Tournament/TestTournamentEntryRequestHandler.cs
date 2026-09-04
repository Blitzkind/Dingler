extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Tournaments;
using HexGame::Game.Client.Network.Tournaments;
using HexGame::Game.Shared.Network.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public sealed class TestTournamentEntryRequestHandler
	: IRequestHandler<TestTournamentEntryRequestArgs, TestTournamentEntryResponse>
{
	private readonly TournamentManager _tournamentManager;

	public TestTournamentEntryRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}

	public TestTournamentEntryResponse HandleRequest(SessionContext context,
		TestTournamentEntryRequestArgs request)
	{
		if ((!_tournamentManager.TryGetTournament(request.TournamentID, out var tournament) || tournament.IsFinished)
		&& !_tournamentManager.TryGetWaitingRoom(request.TournamentID, out _))
		{
			return new TestTournamentEntryResponse()
			{
				success = false,
				qualifyingEntryGroups = new List<int>()
			};
		}

		return new TestTournamentEntryResponse()
		{
			success = true,
			qualifyingEntryGroups = new List<int> { 0 }
		};
	}
}
