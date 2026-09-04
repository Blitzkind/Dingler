namespace Dingler.Game.Protocol.Rooms.Models;

public sealed class RoomUpdate
{
	public int UpdateVersion { get; set; }
	public UpdateType UpdateType { get; set; }
	public object? Payload { get; set; }
	public string? Path { get; set; }

	public RoomUpdate(int updateVersion, UpdateType updateType)
	{
		UpdateVersion = updateVersion;
		UpdateType = updateType;
	}

	public RoomUpdate(int updateVersion, UpdateType updateType, string path, object payload)
		: this(updateVersion, updateType)
	{
		Path = path;
		Payload = payload;
	}

	public RoomUpdate(int updateVersion, object payload)
		: this(updateVersion, UpdateType.Full)
	{
		Payload = payload;
	}
}