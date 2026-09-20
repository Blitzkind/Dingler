using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using Dingler.Game.Protocol.Chat;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class ChatRoomListRequestHandler : IRequestHandler<ChatRoomListRequest>
{
	private readonly GameManager _gameManager;

	public ChatRoomListRequestHandler(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	public void HandleRequest(SessionContext context, ChatRoomListRequest request)
	{ var requester = context.UserName;

		if (requester is null || !_gameManager.TryGetGameForPlayer(requester, out var match))
			return;

		context.TrySendMessageToClient(new RoomListFrame
		{
			Room = request.RawChatRequest.Room,
			Users = match.GetPlayerNames().Select(name => new RoomUserFrame { U = name }).ToList()
		});
	}
}
