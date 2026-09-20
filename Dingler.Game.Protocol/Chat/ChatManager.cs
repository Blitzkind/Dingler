using System.Collections.Concurrent;
using Dingler.Server;
using Dingler.Server.Abstractions;

namespace Dingler.Game.Protocol.Chat;

public class ChatManager : IDisposable
{
	private readonly SessionManager _sessionManager;
	private readonly ChatRoomFactory _chatRoomFactory;
	private readonly ICancellationManager _cancellationManager;
	private readonly ConcurrentDictionary<string, Lazy<ChatRoom>> _chatRooms;
	private readonly ConcurrentDictionary<string, List<ChatRoom>> _chatRoomsPlayerIsIn;

	public ChatManager(SessionManager sessionManager, ChatRoomFactory chatRoomFactory,
		ICancellationManager cancellationManager)
	{
		_sessionManager = sessionManager;
		_chatRoomFactory = chatRoomFactory;
		_cancellationManager = cancellationManager;
		_sessionManager.SessionDisconnected += OnSessionDisconnected;
		_chatRooms = new ConcurrentDictionary<string, Lazy<ChatRoom>>();
		_chatRoomsPlayerIsIn = new ConcurrentDictionary<string, List<ChatRoom>>();
	}

	public Task JoinAsync(string username, string chatRoomName)
	{
		var chatRoom =
			_chatRooms.GetOrAdd(chatRoomName, name => new Lazy<ChatRoom>(() =>
			{
				var room = _chatRoomFactory.Create(name);
				_ = room.StartAsync(_cancellationManager.StoppingToken);
				room.SendMessageToUser += OnSendMessageToUser;
				room.Cleanup += OnCleanup;
				return room;
			})).Value;

		var chatRoomsPlayerIsIn = _chatRoomsPlayerIsIn.GetOrAdd(username, _ => new List<ChatRoom>());

		chatRoomsPlayerIsIn.Add(chatRoom);
		
		return chatRoom.JoinAsync(username);
	}

	public Task SendMessageAsync(RawChatRequest rawChatRequest)
	{
		if (rawChatRequest.Action != "rchat" || !_chatRooms.TryGetValue(rawChatRequest.Room, out var chatRoom))
			return Task.CompletedTask;

		return chatRoom.Value.SendMessageAsync(rawChatRequest.User, rawChatRequest.PlayerIcon, rawChatRequest.Message);
	}

	public Task LeaveAsync(string username, string chatRoomName)
	{
		if (!_chatRooms.TryGetValue(chatRoomName, out var chatRoom))
			return Task.CompletedTask;

		if (!_chatRoomsPlayerIsIn.TryGetValue(username, out var rooms))
			return Task.CompletedTask;

		rooms.Remove(chatRoom.Value);
		
		return chatRoom.Value.LeaveAsync(username);
	}

	private void OnSendMessageToUser(string username, object payload)
	{
		if (!_sessionManager.TryGetUserSession(username, out var sessionContext))
			return;

		sessionContext.TrySendMessageToClient(payload);
	}

	private void OnCleanup(string chatRoomName)
	{
		if (!_chatRooms.Remove(chatRoomName, out var chatRoom))
			return;
		
		chatRoom.Value.Stop();
		chatRoom.Value.Cleanup -= OnCleanup;
		chatRoom.Value.SendMessageToUser -= OnSendMessageToUser;
	}

	private void OnSessionDisconnected(SessionContext context)
	{
		var username = context.UserName;

		if (username is null || !_chatRoomsPlayerIsIn.Remove(username, out var listOfRooms))
			return;

		foreach (var chatRoom in listOfRooms)
		{
			_ = chatRoom.LeaveAsync(username);
		}
	}

	public void Dispose()
	{
		foreach (var kvp in _chatRooms)
		{
			var chatRoom = kvp.Value;
			chatRoom.Value.Cleanup -= OnCleanup;
			chatRoom.Value.SendMessageToUser -= OnSendMessageToUser;
		}

		_sessionManager.SessionDisconnected -= OnSessionDisconnected;
		_chatRooms.Clear();
	}
}