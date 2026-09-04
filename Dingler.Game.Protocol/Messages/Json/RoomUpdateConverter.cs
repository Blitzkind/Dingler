using System.Text.Json;
using System.Text.Json.Serialization;
using Dingler.Game.Protocol.Rooms.Models;

namespace Dingler.Game.Protocol.Messages.Json;

extern alias HexGame;

public sealed class RoomUpdateConverter : JsonConverter<RoomUpdate>
{
	private const string FULL_UPDATE_SYMBOL = "/";
	private const string DELETE_SYMBOL = "del:";
	public override RoomUpdate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}

	public override void Write(Utf8JsonWriter writer, RoomUpdate value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();
		
		writer.WriteRawValue(value.UpdateVersion.ToString());
		
		switch (value.UpdateType)
		{
			case UpdateType.Full:
				writer.WriteStringValue(FULL_UPDATE_SYMBOL);
				JsonSerializer.Serialize(writer, value.Payload, options);
				break;
			case UpdateType.Partial:
				if (value.Path is null)
					throw new InvalidOperationException("Cannot run Partial update without Path");
				writer.WriteStringValue(value.Path);
				JsonSerializer.Serialize(writer, value.Payload, options);
				break;
			case UpdateType.Delete:
				if (value.Path is null)
					throw new InvalidOperationException("Cannot run Delete without Path");
				writer.WriteStringValue($"{DELETE_SYMBOL}{value.Path}");
				writer.WriteStartObject();
				writer.WriteEndObject();
				break;
			default:
				throw new InvalidOperationException($"Unknown UpdateType {value.UpdateType}");
		}
		
		writer.WriteEndArray();
	}
}