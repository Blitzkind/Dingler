using System.Text.RegularExpressions;

namespace Dingler.Game.Protocol.Chat;

public static class ChatMessageConverter
{
	private static readonly Regex TournamentRegex = new(@"^tourn:tournament-(\d+)(?:_([a-zA-Z]+))?$");
	private static readonly Regex WaitingRoomRegex = new(@"^tourn:waitingroom-(\d+)(?:_([a-zA-Z]+))?$");
	private static readonly Regex GameRoomRegex = new(@"^gme:(\d+)$");
	private const string FULL = "full";
	private const string RESUME = "resume";
	private const string JOIN = "rjoin";
	private const string LEAVE = "rleave";
	private const string LIST = "rlist";
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

			if (request.Action.Equals(JOIN))
				return new TournamentJoinChatRequest(id, request, isWaitingRoom,
					isRequestingFullState: options.Equals(FULL));

			if (request.Action.Equals(LEAVE))
				return new TournamentLeaveChatRequest(id, request, isWaitingRoom, isRequestingFullState: options.Equals(FULL));
		}

		if (GameRoomRegex.TryMatch(request.Room, out var gameRoomMatch))
		{
			var sessionId = ulong.Parse(gameRoomMatch.Groups[1].Value);

			if (request.Action.Equals(JOIN))
				return new GameRoomJoinRequest(sessionId, request, isRequestingFullState: false);

			if (request.Action.Equals(LEAVE))
				return new GameRoomLeaveRequest(sessionId, request);

			if (request.Action.Equals(LIST))
				return new GameRoomListRequest(sessionId, request);
		}
		
		return null;
	}
}