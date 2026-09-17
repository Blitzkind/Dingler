extern alias HexGame;
using Dingler.Game.Tournaments;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared.Network.LoadBalancer;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public class QuitOnReconnectionGameRequestHandler : IRequestHandler<QuitOnReconnectionGameRequestArgs>
{
	private readonly TournamentManager _tournamentManager;

	public QuitOnReconnectionGameRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}
	
	public void HandleRequest(SessionContext context, QuitOnReconnectionGameRequestArgs request)
	{
		if (!_tournamentManager.TryGetTournamentPlayerIsIn(context.UserName!, out var currentTournamentId))
			return;
		
		if (!_tournamentManager.TryGetTournament(currentTournamentId, out var tournament))
			return;

		tournament.TryForfeitMatch(context.UserName!);
	}
}