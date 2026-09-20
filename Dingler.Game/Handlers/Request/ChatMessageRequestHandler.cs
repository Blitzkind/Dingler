using Dingler.Game.Protocol.Chat;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;

namespace Dingler.Game.Handlers.Request;

[Authenticated]
public class ChatMessageRequestHandler : IAsyncRequestHandler<ChatMessageRequest>
{
	private readonly ChatManager _chatManager;

	public ChatMessageRequestHandler(ChatManager chatManager)
	{
		_chatManager = chatManager;
	}
	
	public Task HandleRequestAsync(SessionContext context, ChatMessageRequest request, CancellationToken token)
	{
		return _chatManager.SendMessageAsync(request.RawChatRequest);
	}
}