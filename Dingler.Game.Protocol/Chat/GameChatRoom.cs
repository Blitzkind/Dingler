namespace Dingler.Game.Protocol.Chat;

public class GameChatRoom : ChatRoom
{
	public GameChatRoom(string name) : base(name)
	{
	}

	protected override void OnJoin(string joiningUser, IEnumerable<string> usersInRoom)
	{
		var message = new RawChatRequest()
		{
			Action = "rjoin",
			Room = Name,
			User = joiningUser
		};
		
		foreach (var user in usersInRoom)
		{
			OnSendMessageToUser(user, message);
		}
	}

	protected override void OnLeave(string leavingUser, IEnumerable<string> usersInRoom)
	{
		var message = new RawChatRequest()
		{
			Action = "rleave",
			Room = Name,
			User = leavingUser
		};
		
		foreach (var user in usersInRoom)
		{
			OnSendMessageToUser(user, message);
		}
	}
}