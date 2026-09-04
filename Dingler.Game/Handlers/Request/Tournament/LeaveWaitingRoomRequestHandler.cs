extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Tournaments;
using HexGame::Game.Client.Network.Tournaments;
using HexGame::Game.Shared.Network.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public sealed class LeaveWaitingRoomRequestHandler : IAsyncRequestHandler<LeaveWaitingRoomRequestArgs, LeaveWaitingRoomResponse>
{
	private readonly TournamentManager _tournamentManager;

	public LeaveWaitingRoomRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}

	public async Task<LeaveWaitingRoomResponse> HandleRequestAsync(SessionContext context,
		LeaveWaitingRoomRequestArgs request, CancellationToken token)
	{
		if (!_tournamentManager.TryGetTournament(request.WaitingRoomID, out var tournament))
		{
			return new LeaveWaitingRoomResponse()
			{
				Error = ELeaveWaitingRoomError.InvalidTouranmentError,
				ErrorMessage = "Invalid waiting room",
				success = false,
				WaitingRoomID = request.WaitingRoomID
			};
		}

		await tournament.DropFromWaitingRoomAsync(context.UserName!, token);

		return new LeaveWaitingRoomResponse()
		{
			Error = ELeaveWaitingRoomError.Ok,
			ErrorMessage = "Success",
			success = true,
			WaitingRoomID = request.WaitingRoomID
		};
	}
}
