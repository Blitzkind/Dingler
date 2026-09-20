namespace Dingler.Game.Protocol.Chat;

public class ChatRoomState
{
	public HashSet<string> Users { get; }

	public ChatRoomState()
	{
		Users = new HashSet<string>();
	}

	public ChatRoomState(IEnumerable<string> users)
	{
		Users = users.ToHashSet();
	}
}