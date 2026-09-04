extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using Dingler.Game.Protocol.Chat;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class GameRoomListRequestHandler : IRequestHandler<GameRoomListRequest>
{
	private readonly GameManager _gameManager;

	public GameRoomListRequestHandler(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	public void HandleRequest(SessionContext context, GameRoomListRequest request)
	{
		var roomName = "gme:" + request.SessionId;
		var requester = context.UserName;

		if (requester is null || !_gameManager.TryGetGameForPlayer(requester, out var match))
			return;

		context.TrySendMessageToClient(new RoomListFrame
		{
			Room = roomName,
			Users = match.GetPlayerNames().Select(name => new RoomUserFrame { U = name }).ToList()
		});
	}
}
