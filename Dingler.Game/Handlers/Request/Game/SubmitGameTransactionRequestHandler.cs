extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using HexGame::Game.Shared.Network.GameSession;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class SubmitGameTransactionRequestHandler : IRequestHandler<PlayerTransactionRequestArgs>
{
	private readonly GameManager _gameManager;

	public SubmitGameTransactionRequestHandler(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	public void HandleRequest(SessionContext context, PlayerTransactionRequestArgs request)
	{
		if (context.UserName is not null &&
		    _gameManager.TryGetGameForPlayer(context.UserName, out var match))
		{
			match.QueueTransaction(request.Transaction);
		}
	}
}
