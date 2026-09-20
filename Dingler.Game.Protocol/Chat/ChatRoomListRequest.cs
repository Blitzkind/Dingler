namespace Dingler.Game.Protocol.Chat;

public sealed class ChatRoomListRequest : ChatRequest
{
	public ChatRoomListRequest(RawChatRequest rawChatRequest)
		: base(rawChatRequest, isFullStateRequest: false)
	{
	}
}
