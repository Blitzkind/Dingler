namespace Dingler.Game.Protocol.Chat;

public sealed class GameRoomListRequest : ChatRequest
{
	public ulong SessionId { get; }

	public GameRoomListRequest(ulong sessionId, RawChatRequest rawChatRequest)
		: base(rawChatRequest, isFullStateRequest: false)
	{
		SessionId = sessionId;
	}
}
