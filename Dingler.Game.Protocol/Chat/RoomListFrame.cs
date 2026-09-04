using System.Text.Json.Serialization;

namespace Dingler.Game.Protocol.Chat;

public sealed class RoomListFrame
{
	[JsonPropertyName("action")] public string Action { get; } = "rlist";

	[JsonPropertyName("room")] public string Room { get; set; } = "";

	[JsonPropertyName("rflg")] public string Rflg { get; set; } = "";

	[JsonPropertyName("users")] public List<RoomUserFrame> Users { get; set; } = new();
}

public sealed class RoomUserFrame
{
	[JsonPropertyName("u")] public string U { get; set; } = "";

	[JsonPropertyName("f")] public string F { get; set; } = "";
}
