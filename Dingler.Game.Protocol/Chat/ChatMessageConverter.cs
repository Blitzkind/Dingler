using System.Text.RegularExpressions;

namespace Dingler.Game.Protocol.Chat;

public static class ChatMessageConverter
{
	private static readonly Regex TournamentRegex = new(@"^tourn:tournament-(\d+)(?:_([a-zA-Z]+))?$");
	private static readonly Regex WaitingRoomRegex = new(@"^tourn:waitingroom-(\d+)(?:_([a-zA-Z]+))?$");
	private const string FULL = "full";
	private const string RESUME = "resume";
	private const string JOIN = "rjoin";
	private const string LEAVE = "rleave";
	private const string LIST = "rlist";
	private const string CHAT = "rchat";
	
	public static ChatRequest? ParseChatRequest(RawChatRequest request)
	{
		// Fuck this is weird but I'm tired
		var isWaitingRoom = WaitingRoomRegex.TryMatch(request.Room, out var match);
		if (isWaitingRoom || TournamentRegex.TryMatch(request.Room, out match))
		{
			var id = ulong.Parse(match.Groups[1].Value);
			var options = match.Groups[2].Success ? match.Groups[2].Value : "";

			if (options.Equals(RESUME))
				return null;

			switch (request.Action)
			{
				case JOIN:
					return new TournamentJoinChatRequest(id, request, isWaitingRoom,
						isRequestingFullState: options.Equals(FULL));
				case LEAVE:
					return new TournamentLeaveChatRequest(id, request, isWaitingRoom, isRequestingFullState: options.Equals(FULL));
			}
		}

		return request.Action switch
		{
			JOIN => new ChatRoomJoinRequest(request, isRequestingFullState: false),
			LEAVE => new ChatRoomLeaveRequest(request),
			LIST => new ChatRoomListRequest(request),
			CHAT => new ChatMessageRequest(request),
			_ => null
		};
	}
}