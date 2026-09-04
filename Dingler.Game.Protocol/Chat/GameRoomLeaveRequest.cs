namespace Dingler.Game.Protocol.Chat;

public sealed class GameRoomLeaveRequest : ChatRequest
{
	public ulong SessionId { get; }

	public GameRoomLeaveRequest(ulong sessionId, RawChatRequest rawChatRequest)
		: base(rawChatRequest, isFullStateRequest: false)
	{
		SessionId = sessionId;
	}
}
