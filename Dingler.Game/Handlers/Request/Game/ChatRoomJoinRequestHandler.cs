using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Protocol.Chat;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class ChatRoomJoinRequestHandler : IAsyncRequestHandler<ChatRoomJoinRequest>
{
	private readonly ChatManager _chatManager;

	public ChatRoomJoinRequestHandler(ChatManager chatManager)
	{
		_chatManager = chatManager;
	}

	public async Task HandleRequestAsync(SessionContext context, ChatRoomJoinRequest request, CancellationToken token)
	{
		var roomName = request.RawChatRequest.Room;
		var joiner = context.UserName!;
		await _chatManager.JoinAsync(joiner, roomName);
	}
}
