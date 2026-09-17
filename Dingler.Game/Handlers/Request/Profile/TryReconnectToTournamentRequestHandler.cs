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
	private readonly TournamentManager _tournamentManager;
	public TryReconnectToTournamentRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}
	
	public async Task<TryReconnectionToDisconnectedTournamentResponse> HandleRequestAsync(SessionContext context,
		TryReconnectionToDisconnectedTournamentRequestArgs request, CancellationToken token)
	{
		if (!await _tournamentManager.ReconnectAsync(context) ||
		    !_tournamentManager.TryGetTournament(context.CurrentTournamentId, out var tournament))  
			return new TryReconnectionToDisconnectedTournamentResponse()
			{
				ErrorMessage = "No tournament",
				Error = ETryReconnectionToDisconnectedTournamentError.Ok,
			};

		var tournamentInfo = await tournament.GetInfoAsync();

		context.TrySendMessageToClient(new TournamentInfoEventArgs(tournamentInfo));
		
		return new TryReconnectionToDisconnectedTournamentResponse();
	}
}