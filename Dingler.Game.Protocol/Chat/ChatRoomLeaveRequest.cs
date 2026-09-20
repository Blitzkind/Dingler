namespace Dingler.Game.Protocol.Chat;

public sealed class ChatRoomLeaveRequest : ChatRequest
{
	public ChatRoomLeaveRequest(RawChatRequest rawChatRequest)
		: base(rawChatRequest, isFullStateRequest: false)
	{
		
	}
}
