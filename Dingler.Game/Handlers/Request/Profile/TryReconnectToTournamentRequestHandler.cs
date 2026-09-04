extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Tournaments;
using HexGame::Game.Shared.Network.Tournaments;
using HexGame::Game.Client.Network.Tournaments;

namespace Dingler.Game.Handlers.Request.Profile;

[Authenticated]
public sealed class TryReconnectToTournamentRequestHandler
	: IAsyncRequestHandler<TryReconnectionToDisconnectedTournamentRequestArgs,
		TryReconnectionToDisconnectedTournamentResponse>
{
	private TournamentManager _tournamentManager;
	public TryReconnectToTournamentRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}
	
	public async Task<TryReconnectionToDisconnectedTournamentResponse> HandleRequestAsync(SessionContext context,
		TryReconnectionToDisconnectedTournamentRequestArgs request, CancellationToken token)
	{
		if (!await _tournamentManager.ReconnectAsync(context)) 
			return new TryReconnectionToDisconnectedTournamentResponse();

		return new TryReconnectionToDisconnectedTournamentResponse();
	}
}