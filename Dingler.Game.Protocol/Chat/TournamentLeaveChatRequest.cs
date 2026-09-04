namespace Dingler.Game.Protocol.Chat;

public class TournamentLeaveChatRequest : ChatRequest
{
	public ulong TournamentId { get; }
	public bool IsWaitingRoom { get; }

	public TournamentLeaveChatRequest(ulong tournamentId, RawChatRequest rawChatRequest, bool isWaitingRoom = false,
		bool isRequestingFullState = false) : base(rawChatRequest, isRequestingFullState)

	{
		TournamentId = tournamentId;
		IsWaitingRoom = isWaitingRoom;
	}
}