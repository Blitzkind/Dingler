extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Tournaments;
using HexGame::Game.Client.Network.Tournaments;
using HexGame::Game.Shared.Network.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public sealed class LeaveTournamentTransactionRequestHandler
	: IAsyncRequestHandler<LeaveTournamentRequestArgs, LeaveTournamentResponse>
{
	private readonly TournamentManager _tournamentManager;

	public LeaveTournamentTransactionRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}

	public async Task<LeaveTournamentResponse> HandleRequestAsync(SessionContext context,
		LeaveTournamentRequestArgs request, CancellationToken token)
	{
		if (!_tournamentManager.TryGetTournament(request.TournamentID, out var tournament))
		{
			return new LeaveTournamentResponse()
			{
				Error = ELeaveTournamentError.InvalidTouranmentError,
				ErrorMessage = "Invalid tournament",
				success = false,
				TournamentID = request.TournamentID
			};
		}

		await tournament.DropAsync(context.UserName!);

		return new LeaveTournamentResponse()
		{
			Error = ELeaveTournamentError.Ok,
			ErrorMessage = "Success",
			success = true,
			TournamentID = request.TournamentID
		};
	}
}
