extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Tournaments;
using HexGame::Game.Client.Network.Tournaments;
using HexGame::Game.Shared.Network.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public class SaveTournamentDeckRequestHandler : IRequestHandler<SaveTournamentDeckRequestArgs, SaveTournamentDeckResponse>
{
	private readonly TournamentManager _tournamentManager;

	public SaveTournamentDeckRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}

	public SaveTournamentDeckResponse HandleRequest(SessionContext context,
		SaveTournamentDeckRequestArgs request)
	{
		if (!_tournamentManager.TryGetTournament(request.TournamentID, out var tournament))
		{
			return new SaveTournamentDeckResponse()
			{
				Success = false,
				Error = ESaveTournamentDeckError.InternalServerError,
				ErrorMessage = "Tournament does not exist"
			};
		}

		tournament.UpdateDeck(context.UserName!, request.TournamentDeck);

		return new SaveTournamentDeckResponse()
		{
			Success = true,
			Error = ESaveTournamentDeckError.Ok
		};
	}
}