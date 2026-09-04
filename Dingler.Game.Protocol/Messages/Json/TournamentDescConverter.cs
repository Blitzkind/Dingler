extern alias HexGame;

using System.Text.Json;
using System.Text.Json.Serialization;
using HexGame::Game.Shared.Tournaments;

namespace Dingler.Game.Protocol.Messages.Json
{
    public class TournamentDescConverter : JsonConverter<TournamentDesc>
    {
        public override TournamentDesc? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException(
                "Json deserialziation of TournamentDesc is not accepted. Use Hex Custom Decoder.");
        }

        public override void Write(Utf8JsonWriter writer, TournamentDesc value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("roomType", value.RoomType);
            writer.WriteNumber("tournamentID", value.TournamentID);
            writer.WriteString("description", value.Description);
            writer.WriteNumber("numPlayers", value.NumberPlayers);
            writer.WriteNumber("maxPlayers", value.MaxPlayers);
            writer.WriteNumber("minPlayers", value.MinPlayers);
            writer.WriteNumber("maxRounds", value.MaxRounds);
            writer.WriteNumber("style", (int)value.Style);
            writer.WriteNumber("endTime", value.TournamentEndTime);
            writer.WriteNumber("startTime", value.TournamentStartTime);
            writer.WriteNumber("openTime", value.RegistrationOpenTime);
            writer.WriteNumber("lastUpdate", value.LastUpdateTime);
            writer.WriteNumber("format", (int)value.Format);
            writer.WriteString("state", value.TournamentState.FullPath);
            writer.WriteNumber("tournamentStatus", (int)value.TournamentStatus);
            writer.WriteNumber("completionType", (int)value.CompletionType);
            writer.WriteNumber("currentRound", value.CurrentRound);
            writer.WriteNumber("requiredTOS", value.requiredTOS);
            if (value.Players != null)
            {
                writer.WritePropertyName("Players");
                JsonSerializer.Serialize(writer, value.Players, options);
            }
            if (value.linkedTournament != null)
            {
                writer.WritePropertyName("linkedTournament");
                JsonSerializer.Serialize(writer, value.linkedTournament, options);
            }
            writer.WritePropertyName("tournamentFees");
            if (value.tournamentFees != null)
                JsonSerializer.Serialize(writer, value.tournamentFees, options);
            else
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }
            
            writer.WritePropertyName("tournamentRewards");
            if (value.tournamentRewards != null)
                JsonSerializer.Serialize(writer, value.tournamentRewards, options);
            else
            {
                writer.WriteStartObject();
                writer.WritePropertyName("tournamentRewards");
                writer.WriteStartArray();
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }
    }
}
