extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Extensions;
using Dingler.Game.Games;
using Dingler.Game.Protocol.Chat;
using Dingler.Game.Tournaments;
using Microsoft.Extensions.Logging;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class GameRoomLeaveRequestHandler : IRequestHandler<GameRoomLeaveRequest>
{
	private readonly GameManager _gameManager;
	private readonly TournamentManager _tournamentManager;
	private readonly SessionManager _sessionManager;
	private readonly ILogger<GameRoomLeaveRequestHandler>? _logger;

	public GameRoomLeaveRequestHandler(GameManager gameManager, TournamentManager tournamentManager, SessionManager sessionManager,
		ILogger<GameRoomLeaveRequestHandler>? logger = null)
	{
		_gameManager = gameManager;
		_tournamentManager = tournamentManager;
		_sessionManager = sessionManager;
		_logger = logger;
	}

	public void HandleRequest(SessionContext context, GameRoomLeaveRequest request)
	{
		_logger?.LogDebug("Got room leave request from {username}", context.UserName!);
		var roomName = "gme:" + request.SessionId;
		var leaver = context.UserName;

		context.TrySendMessageToClient(request.RawChatRequest);

		if (leaver is null)
			return;

		if (!_gameManager.TryGetGameForPlayer(leaver, out var game))
		{
			if (context.TryGetCurrentTournamentId(out var tournamentId) &&
			    _tournamentManager.TryGetTournament(tournamentId, out var tournament))
			{
				tournament.HandlePlayerWantsToLeave(leaver);
			}
			return;
		}

		var opponent = game.GetPlayerNames().FirstOrDefault(name => name != leaver);
		if (opponent is not null && _sessionManager.TryGetUserSession(opponent, out var opponentSession))
		{
			opponentSession.TrySendMessageToClient(new RawChatRequest
			{
				Action = "rleave",
				Room = roomName,
				User = leaver
			});
		}
	}
}
