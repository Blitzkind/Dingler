extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using HexGame::Game.Shared.Network.LoadBalancer;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class ReadyForGameEventsRequestHandler : IRequestHandler<ReadyForGameEventsRequestArgs>
{
	private readonly GameManager _gameManager;

	public ReadyForGameEventsRequestHandler(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	public void HandleRequest(SessionContext context, ReadyForGameEventsRequestArgs request)
	{
		if (context.UserName is not null &&
		    _gameManager.TryGetGameForPlayer(context.UserName, out var match))
		{
			match.PlayerIsReadyForEvents(request.PlayerId);
		}
	}
}
