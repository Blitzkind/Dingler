extern alias HexGame;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dingler.Game.Protocol.Rooms.Models;

namespace Dingler.Game.Protocol.Messages.Json;

public sealed class WaitingRoomUpdateConverter : JsonConverter<WaitingRoomUpdate>
{
	public override WaitingRoomUpdate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}

	public override void Write(Utf8JsonWriter writer, WaitingRoomUpdate value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WritePropertyName($"tourn:waitingroom-{value.Id}");
		writer.WriteStartObject();
		writer.WritePropertyName("players");
		writer.WriteRawValue(JsonSerializer.Serialize(value.Players, options));
		writer.WritePropertyName("numberOfPlayers");
		writer.WriteRawValue(JsonSerializer.Serialize(value.Players.Count, options));
		writer.WriteEndObject();
		writer.WriteEndObject();
	}
}