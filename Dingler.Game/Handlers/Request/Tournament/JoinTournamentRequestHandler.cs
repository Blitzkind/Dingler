extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Extensions;
using Dingler.Game.Protocol.Chat;
using Dingler.Game.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public sealed class JoinTournamentRequestHandler : IAsyncRequestHandler<TournamentJoinChatRequest>
{
	private readonly TournamentManager _tournamentManager;

	public JoinTournamentRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}

	public async Task HandleRequestAsync(SessionContext context, TournamentJoinChatRequest request, CancellationToken token)
	{
		context.TrySendMessageToClient(request.RawChatRequest);

		if (!_tournamentManager.TryGetJoinableTournament(request.TournamentId, out var tournament))
			return;
		
		var result = await tournament.TryJoinAsync(context, request, token);

		if (result)
		{
			context.SetCurrentTournamentId(request.TournamentId);
		}
	}
}
