namespace Dingler.Game.Protocol.Chat;

public sealed class GameRoomJoinRequest : ChatRequest
{
	public ulong SessionId { get; }

	public GameRoomJoinRequest(ulong sessionId, RawChatRequest rawChatRequest, bool isRequestingFullState)
		: base(rawChatRequest, isRequestingFullState)
	{
		SessionId = sessionId;
	}
}
