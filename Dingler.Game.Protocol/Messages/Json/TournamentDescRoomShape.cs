extern alias HexGame;

using System.Text.Json.Serialization;
using HexGame::Game.Shared.Tournaments;
using HexGame::Game.Shared.TournamentSystemMkII;

namespace Dingler.Game.Protocol.Messages.Json;

public sealed class TournamentDescRoomShape
{
	[JsonPropertyName("roomType")] public string RoomType { get; set; } = "";

	[JsonPropertyName("id")] public ulong Id { get; set; }

	[JsonPropertyName("name")] public string Name { get; set; } = "";

	[JsonPropertyName("numPlayers")] public int NumPlayers { get; set; }

	[JsonPropertyName("maxPlayers")] public int MaxPlayers { get; set; }

	[JsonPropertyName("minPlayers")] public int MinPlayers { get; set; }

	[JsonPropertyName("maxRounds")] public int MaxRounds { get; set; }

	[JsonPropertyName("currentRound")] public int CurrentRound { get; set; }

	[JsonPropertyName("format")] public int Format { get; set; }

	[JsonPropertyName("style")] public int Style { get; set; }

	[JsonPropertyName("state")] public string State { get; set; } = "";

	[JsonPropertyName("startTime")] public long StartTime { get; set; }

	[JsonPropertyName("endTime")] public long EndTime { get; set; }

	[JsonPropertyName("openTime")] public long OpenTime { get; set; }

	[JsonPropertyName("lastUpdate")] public long LastUpdate { get; set; }

	[JsonPropertyName("requiredTOS")] public int RequiredTOS { get; set; }

	[JsonPropertyName("rewards")] public TournamentRewardCollection? Rewards { get; set; }

	[JsonPropertyName("fees")] public Dictionary<int, TournamentEntryInfo>? Fees { get; set; }

	public static TournamentDescRoomShape FromTournamentDesc(TournamentDesc desc)
	{
		return new TournamentDescRoomShape
		{
			RoomType = desc.RoomType ?? "",
			Id = desc.TournamentID,
			Name = desc.Description,
			NumPlayers = desc.NumberPlayers,
			MaxPlayers = desc.MaxPlayers,
			MinPlayers = desc.MinPlayers,
			MaxRounds = desc.MaxRounds,
			CurrentRound = desc.CurrentRound,
			Format = (int)desc.Format,
			Style = (int)desc.Style,
			State = desc.TournamentState?.FullPath ?? TournamentState.WaitForStart.FullPath,
			StartTime = desc.TournamentStartTime,
			EndTime = desc.TournamentEndTime,
			OpenTime = desc.RegistrationOpenTime,
			LastUpdate = desc.LastUpdateTime,
			RequiredTOS = desc.requiredTOS,
			Rewards = desc.tournamentRewards ?? new TournamentRewardCollection(),
			Fees = desc.tournamentFees ?? new Dictionary<int, TournamentEntryInfo>()
		};
	}
}
