using Dingler.Server.Systems;

namespace Dingler.Game.Protocol.Chat;

public class ChatRoom
{
	protected string Name { get; }
	protected readonly Actor<ChatRoomState> _actor;
	public event Action<string, object>? SendMessageToUser;
	public event Action<string>? Cleanup;

	public ChatRoom(string name)
	{
		Name = name;
		_actor = new Actor<ChatRoomState>(new ChatRoomState());
	}

	public Task StartAsync(CancellationToken token)
	{
		return _actor.RunAsync(token);
	}

	public void Stop()
	{
		_actor.Finish();
	}
	
	public Task JoinAsync(string username)
	{
		return _actor.ScheduleWork(state =>
		{
			if (!state.Users.Add(username))
				return;
			
			OnJoin(username, state.Users);
		});
	}

	public Task LeaveAsync(string username)
	{
		return _actor.ScheduleWork(state =>
		{
			if (!state.Users.Remove(username))
				return;

			OnLeave(username, state.Users);
			
			if (state.Users.Count == 0)
				Cleanup?.Invoke(Name);
		});
	}

	public Task SendMessageAsync(string sendingUser, string icon, string message)
	{
		return _actor.ScheduleWork(state =>
		{
			if (!state.Users.Contains(sendingUser))
				return;

			foreach (var user in state.Users)
			{
				OnSendMessageToUser(user, new RawChatRequest()
				{
					Action = "rchat",
					Message = message,
					PlayerIcon = icon,
					Room = Name, 
					User = sendingUser
				});
			}
		});
	}

	protected virtual void OnJoin(string joiningUser, IEnumerable<string> usersInRoom)
	{
		OnSendMessageToUser(joiningUser, new RawChatRequest()
		{
			Action = "rjoin",
			Room = Name,
			User = joiningUser
		});
	}

	protected virtual void OnLeave(string leavingUser, IEnumerable<string> usersInRoom)
	{
		OnSendMessageToUser(leavingUser, new RawChatRequest()
		{
			Action = "rleave",
			Room = Name,
			User = leavingUser
		});
	}

	protected void OnSendMessageToUser(string username, object payload)
	{
		SendMessageToUser?.Invoke(username, payload);
	}
}