extern alias HexGame;
using System.Text.Json;
using System.Text.Json.Serialization;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Protocol.Messages.Json;

public class TournamentPlayerInfoConverter : JsonConverter<TournamentPlayerInfo>
{
	public override TournamentPlayerInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}

	public override void Write(Utf8JsonWriter writer, TournamentPlayerInfo value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteString("state", value.State.FullPath);
		writer.WriteString("id", $"p{value.PlayerID}");
		writer.WriteNumber("uid", value.PlayerUID.GetInstanceId());
		writer.WriteString("deckid", value.DeckHash ?? "");
		writer.WriteString("name", value.Name);
		writer.WriteNumber("points", value.Points);
		writer.WriteNumber("wins", value.Wins);
		writer.WriteNumber("losses", value.Losses);
		writer.WriteNumber("eliminationRound", value.ElimintationRound);
		writer.WriteNumber("eliminationReason", (long)value.EliminationReason);
		writer.WriteNumber("rank", value.Rank);
		writer.WriteNumber("gwr", value.GWR);
		writer.WriteNumber("omwr", value.OMWR);
		writer.WriteNumber("oomwr", value.OOMWR);
		writer.WriteEndObject();
	}
}