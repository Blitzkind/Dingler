namespace Dingler.Game.Protocol.Rooms.Models;

public sealed class RoomData
{
	public string Room { get; set; }
	public string Flags { get; }
	public string RoomFlags { get; }
	public string Sender { get; }
	
	public List<RoomUpdate> Updates { get; set; }

	public RoomData(string room, string sender)
	{
		Room = room;
		Flags = "";
		RoomFlags = "";
		Sender = sender;

		Updates = new List<RoomUpdate>();
	}

	public RoomData(string room, string sender, ICollection<RoomUpdate> updates)
		: this(room, sender)
	{
		Updates = updates.ToList();
	}

	public RoomData(string room, string sender, RoomUpdate update)
		: this(room, sender)
	{
		Updates.Add(update);
	}
}