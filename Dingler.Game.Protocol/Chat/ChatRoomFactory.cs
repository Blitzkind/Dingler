using System.Diagnostics;
using Dingler.Server;
using Dingler.Server.Abstractions;

namespace Dingler.Game.Protocol.Chat;

extern alias HexGame;

public class ChatRoomFactory
{
	private readonly ICancellationManager _cancellationManager;

	public ChatRoomFactory(ICancellationManager cancellationManager)
	{
		_cancellationManager = cancellationManager;
	}
	
	
	public ChatRoom Create(string roomName)
	{
		if (roomName.StartsWith("gme"))
			return new GameChatRoom(roomName);
		
		return new ChatRoom(roomName);
	}
}