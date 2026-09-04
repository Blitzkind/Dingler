namespace Dingler.Game.Protocol.Chat;

public class TournamentJoinChatRequest : ChatRequest
{
	public ulong TournamentId { get; }
	public bool IsWaitingRoomRequest { get; }
	
	public TournamentJoinChatRequest(ulong tournamentId, RawChatRequest rawChatRequest, bool isWaitingRoom = false, bool isRequestingFullState = false) 
		: base(rawChatRequest, isRequestingFullState)
	{
		TournamentId = tournamentId;
		IsWaitingRoomRequest = isWaitingRoom;
	}

}