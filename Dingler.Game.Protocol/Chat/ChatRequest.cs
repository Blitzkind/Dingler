namespace Dingler.Game.Protocol.Chat;

public abstract class ChatRequest
{
	public RawChatRequest RawChatRequest { get; }
	public bool IsFullStateRequest { get; }

	protected ChatRequest(RawChatRequest rawChatRequest, bool isFullStateRequest)
	{
		RawChatRequest = rawChatRequest;
		IsFullStateRequest = isFullStateRequest;
	}
}