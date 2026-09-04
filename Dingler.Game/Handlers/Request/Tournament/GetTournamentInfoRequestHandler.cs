extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Tournaments;
using HexGame::Game.Client.Network.Tournaments;
using HexGame::Game.Shared.Network.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public sealed class GetTournamentInfoRequestHandler
	: IAsyncRequestHandler<GetTournamentInfoRequestArgs, GetTournamentInfoResponse>
{
	private readonly TournamentManager _tournamentManager;

	public GetTournamentInfoRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}

	public async Task<GetTournamentInfoResponse> HandleRequestAsync(SessionContext context,
		GetTournamentInfoRequestArgs request, CancellationToken token)
	{
		if (!_tournamentManager.TryGetTournament(request.TournamentID, out var tournament))
		{
			return new GetTournamentInfoResponse()
			{
				Error = EGetTournamentInfoError.InvalidTouranmentError,
				ErrorMessage = "Invalid tournament"
			};
		}

		var info = await tournament.GetInfoAsync();

		return new GetTournamentInfoResponse()
		{
			Error = EGetTournamentInfoError.Ok,
			ErrorMessage = "Success",
			Results = info
		};
	}
}
