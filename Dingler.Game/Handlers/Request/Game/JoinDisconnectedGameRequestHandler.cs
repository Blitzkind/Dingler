extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using HexGame::Game.Client.Network.LoadBalancer;
using HexGame::Game.Shared.Network.LoadBalancer;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class JoinDisconnectedGameRequestHandler
	: IRequestHandler<JoinDisconnectedGameRequestArgs, JoinDisconnectedGameResponse>
{
	private readonly GameManager _gameManager;

	public JoinDisconnectedGameRequestHandler(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	public JoinDisconnectedGameResponse HandleRequest(SessionContext context,
		JoinDisconnectedGameRequestArgs request)
	{
		if (context.UserName is null ||
		    !_gameManager.TryGetGameForPlayer(context.UserName, out _))
			return new JoinDisconnectedGameResponse { Error = EJoinDisconnectedGameError.InternalServerError };

		return new JoinDisconnectedGameResponse { RoutingPlayerId = request.PlayerId };
	}
}
