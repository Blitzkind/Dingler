using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using Dingler.Game.Protocol.Chat;
using Dingler.Game.Tournaments;
using Microsoft.Extensions.Logging;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class ChatRoomLeaveRequestHandler : IAsyncRequestHandler<ChatRoomLeaveRequest>
{
	private readonly GameManager _gameManager;
	private readonly TournamentManager _tournamentManager;
	private readonly ChatManager _chatManager;
	private readonly ILogger<ChatRoomLeaveRequestHandler>? _logger;

	public ChatRoomLeaveRequestHandler(GameManager gameManager, TournamentManager tournamentManager,
		ChatManager chatManager, ILogger<ChatRoomLeaveRequestHandler>? logger = null)
	{
		_gameManager = gameManager;
		_tournamentManager = tournamentManager;
		_chatManager = chatManager;
		_logger = logger;
	}

	public async Task HandleRequestAsync(SessionContext context, ChatRoomLeaveRequest request, CancellationToken token)
	{
		_logger?.LogDebug("Got room leave request from {username}", context.UserName!);
		var roomName = request.RawChatRequest.Room;
		var leaver = context.UserName;

		if (leaver is null)
			return;

		await _chatManager.LeaveAsync(leaver, roomName);
		
		if (!_tournamentManager.TryGetTournament(context.CurrentTournamentId, out var tournament))
			return;
		
		tournament.HandlePlayerWantsToLeave(leaver);
	}
}
