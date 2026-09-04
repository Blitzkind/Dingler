namespace Dingler.Game.Protocol.Rooms.Models;

public sealed class WaitingRoomUpdate
{
	public ulong Id { get; }
	public List<string> Players { get; }

	public WaitingRoomUpdate(ulong id)
	{
		Id = id;
		Players = [];
	}

	public WaitingRoomUpdate(ulong id, IEnumerable<string> players)
	{
		Id = id;
		Players = players.ToList();
	}

	public WaitingRoomUpdate(ulong id, string player)
	{
		Id = id;
		Players = [player];
	}
}