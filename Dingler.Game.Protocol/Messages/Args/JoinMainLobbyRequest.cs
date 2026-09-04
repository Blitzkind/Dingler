namespace Dingler.Game.Protocol.Messages.Args;

public class JoinMainLobbyRequest
{
	public bool IsFullRequest { get; }

	public JoinMainLobbyRequest(bool isFullRequest)
	{
		IsFullRequest = isFullRequest;
	}
}