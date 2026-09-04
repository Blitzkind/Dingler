extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using HexGame::Game.Client.Network.LoadBalancer;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Network.LoadBalancer;

namespace Dingler.Game.Handlers.Request.Profile;

[Authenticated]
public sealed class TryReconnectToGameRequestHandler : IRequestHandler<TryReconnectionToDisconnectedGameRequestArgs,
	TryReconnectionToDisconnectedGameResponse>
{
	private readonly GameManager _gameManager;
	private readonly TryReconnectionToDisconnectedGameResponse _nullResponse;

	public TryReconnectToGameRequestHandler(GameManager gameManager)
	{
		_gameManager = gameManager;
		_nullResponse = new TryReconnectionToDisconnectedGameResponse()
		{
			SessionState = null
		};
	}
	
	public TryReconnectionToDisconnectedGameResponse HandleRequest(SessionContext context,
		TryReconnectionToDisconnectedGameRequestArgs request)
	{
		if (!_gameManager.TryGetGameForPlayer(context.UserName!, out var session))
			return _nullResponse;

		return new TryReconnectionToDisconnectedGameResponse()
		{
			SessionState = new SessionState()
			{
				MaximumPlayerCount = 2,
				MinimumPlayerCount = 2,
				JoinInsteadOfReconnect = false,
				SessionName = $"game-4",
			}
		};
	}
}