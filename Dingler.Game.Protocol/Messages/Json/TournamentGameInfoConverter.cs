extern alias HexGame;
using System.Text.Json;
using System.Text.Json.Serialization;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Protocol.Messages.Json;

public class TournamentGameInfoConverter : JsonConverter<TournamentGameInfo>
{
	public override TournamentGameInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}

	public override void Write(Utf8JsonWriter writer, TournamentGameInfo value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteString("state", value.State.FullPath);
		writer.WriteNumber("matchID", value.MatchID);
		writer.WriteNumber("roundID", value.RoundID);
		writer.WriteString("player1id", $"p{value.Player1ID}");
		writer.WriteString("player2id", $"p{value.Player2ID}");
		writer.WriteNumber("startTime", value.StartTime);
		writer.WriteNumber("endTime", value.EndTime);
		writer.WriteNumber("game1Winner", value.Game1Winner);
		writer.WriteNumber("game2Winner", value.Game2Winner);
		writer.WriteNumber("game3Winner", value.Game3Winner);
		writer.WriteEndObject();
	}
}