using System.Text.Json.Serialization;

namespace Dingler.Game.Protocol.Chat;

public sealed class RawChatRequest
{
	[JsonPropertyName("action")] public string Action { get; set; } = "";

	[JsonPropertyName("room")] public string Room { get; set; } = "";

	[JsonPropertyName("pass")] public string Password { get; set; } = "";

	[JsonPropertyName("flags")] public string Flags { get; set; } = "";

	[JsonPropertyName("msg")] public string Message { get; set; } = "";

	[JsonPropertyName("icon")] public string PlayerIcon { get; set; } = "";

	[JsonPropertyName("rflg")] public string RoomFlags { get; set; } = "";
	
	[JsonPropertyName("user")] public string User { get; set; } = "";
}