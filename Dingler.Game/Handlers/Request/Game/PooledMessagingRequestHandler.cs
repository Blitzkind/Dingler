extern alias HexGame;
using System.Text.Json;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Tournaments;
using HexGame::Game.Shared.PooledMessaging;

namespace Dingler.Game.Handlers.Request.Game;

// I wrote this so long ago I don't remember what it's for. *Gandalf meme*
[Authenticated]
public sealed class PooledMessagingRequestHandler : IRequestHandler<PooledMessagingRequestsInterface.Request, PooledMessagingRequestsInterface.Response>
{
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		IncludeFields = true,
		PropertyNameCaseInsensitive = true
	};

	private readonly TournamentManager _tournamentManager;

	public PooledMessagingRequestHandler(TournamentManager tournamentManager)
	{
		_tournamentManager = tournamentManager;
	}

	public PooledMessagingRequestsInterface.Response HandleRequest(SessionContext context,
		PooledMessagingRequestsInterface.Request request)
	{
		return new PooledMessagingRequestsInterface.Response();
	}
}
