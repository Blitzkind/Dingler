extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using Dingler.Game.Protocol.Chat;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class GameRoomJoinRequestHandler : IRequestHandler<GameRoomJoinRequest>
{
	private readonly GameManager _gameManager;
	private readonly SessionManager _sessionManager;

	public GameRoomJoinRequestHandler(GameManager gameManager, SessionManager sessionManager)
	{
		_gameManager = gameManager;
		_sessionManager = sessionManager;
	}

	public void HandleRequest(SessionContext context, GameRoomJoinRequest request)
	{
		var roomName = "gme:" + request.SessionId;
		var joiner = context.UserName;

		context.TrySendMessageToClient(request.RawChatRequest);

		if (joiner is null || !_gameManager.TryGetGameForPlayer(joiner, out var match))
			return;
		
		var opponent = match.GetPlayerNames().FirstOrDefault(name => name != joiner);
		if (opponent is not null && _sessionManager.TryGetUserSession(opponent, out var opponentSession))
		{
			opponentSession.TrySendMessageToClient(new RawChatRequest
			{
				Action = "rjoin",
				Room = roomName,
				User = joiner
			});
		}

		context.TrySendMessageToClient(new RoomListFrame
		{
			Room = roomName,
			Users = match.GetPlayerNames().Select(name => new RoomUserFrame { U = name }).ToList()
		});
	}
}
