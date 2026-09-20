namespace Dingler.Game.Protocol.Chat;

public sealed class ChatRoomJoinRequest : ChatRequest
{

	public ChatRoomJoinRequest(RawChatRequest rawChatRequest, bool isRequestingFullState)
		: base(rawChatRequest, isRequestingFullState)
	{
		
	}
}
