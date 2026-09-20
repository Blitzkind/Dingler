namespace Dingler.Game.Protocol.Chat;

public class ChatMessageRequest : ChatRequest
{
	public ChatMessageRequest(RawChatRequest rawChatRequest) : base(rawChatRequest, isFullStateRequest: false)
	{
	}
}