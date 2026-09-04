extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Protocol.Chat;
using Dingler.Game.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public sealed class LeaveTournamentRequestHandler : IRequestHandler<TournamentLeaveChatRequest>
{
	private readonly TournamentManager _tournamentManager;

	public LeaveTournamentRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}
	
	public void HandleRequest(SessionContext context, TournamentLeaveChatRequest request)
	{
		context.TrySendMessageToClient(request.RawChatRequest);
		
		if (request.IsFullStateRequest || request.IsWaitingRoom)
			return;
		
		if (!_tournamentManager.TryGetTournament(request.TournamentId, out var tournament))
			return;

		tournament.TryForfeitMatch(context.UserName!);
	}
}
