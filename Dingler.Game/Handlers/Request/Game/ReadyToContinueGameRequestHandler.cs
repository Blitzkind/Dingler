extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using Dingler.Game.Tournaments;
using HexGame::Game.Client.Network.LoadBalancer;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Network.LoadBalancer;
using HexGame::Game.Shared.Network.Tournaments;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class ReadyToContinueGameRequestHandler
	: IAsyncRequestHandler<ReadyToContinueGameRequestArgs, ReadyToContinueGameResponse>
{
	private readonly GameManager _gameManager;
	private readonly TournamentManager _tournamentManager;

	public ReadyToContinueGameRequestHandler(GameManager gameManager, TournamentManager tournamentManager)
	{
		_gameManager = gameManager;
		_tournamentManager = tournamentManager;
	}

	public async Task<ReadyToContinueGameResponse> HandleRequestAsync(SessionContext context,
		ReadyToContinueGameRequestArgs request, CancellationToken token)
	{
		if (context.UserName is not null && request.IsReady &&
		    _gameManager.TryGetGameForPlayer(context.UserName, out var match))
		{
			match.ReconnectPlayer(request.PlayerId);
			await SendTournamentStateAsync(context, match).ConfigureAwait(false);
		}

		return new ReadyToContinueGameResponse();
	}

	private async Task SendTournamentStateAsync(SessionContext context, HexGameWrapper match)
	{
		var encounterData = match.EncounterData;
		if ((encounterData.SessionFlags & ESessionFlags.IsTournament) == ESessionFlags.None ||
		    !_tournamentManager.TryGetTournament(encounterData.TournamentID, out var tournament))
		{
			return;
		}

		context.CurrentTournamentId = encounterData.TournamentID;

		var info = await tournament.GetInfoAsync().ConfigureAwait(false);

		context.TrySendMessageToClient(new TournamentInfoEventArgs(new TournamentInfo
		{
			TournamentID = info.TournamentID,
			TournamentStatus = info.TournamentStatus,
			Format = info.Format,
			Style = info.Style,
			MinEntrants = info.MinEntrants,
			MaxEntrants = info.MaxEntrants,
			Games = new List<TournamentGameInfo>(),
			Players = new List<TournamentPlayerInfo>()
		}));

		var deckEntry = encounterData.TournamentDecks?.FirstOrDefault(d => d.PlayerName == context.UserName);
		context.TrySendMessageToClient(new TournamentPlayerJoinedEventArgs(encounterData.TournamentID, false, deckEntry?.PlayerDeck, null));
	}
}
