extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Games;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Network.LoadBalancer;

namespace Dingler.Game.Handlers.Request.Game;

[Authenticated]
public sealed class ReadyForGameSetupRequestHandler : IRequestHandler<ReadyForGameSetupRequestArgs, ReadyForGameSetupResponseArgs>
{
	private readonly GameManager _gameManager;

	public ReadyForGameSetupRequestHandler(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	public ReadyForGameSetupResponseArgs HandleRequest(SessionContext context,
		ReadyForGameSetupRequestArgs request)
	{
		if (context.UserName is null ||
		    !_gameManager.TryGetGameForPlayer(context.UserName, out var match))
		{
			return new ReadyForGameSetupResponseArgs
			{
				SessionState = null,
				DeckId = UID.Invalid,
				DeckTemplateId = ResourceId.Invalid,
				OpponentsInfo = new List<PlayerState>(),
				TurnOrder = new List<UID>(),
				seedZ = 0,
				seedW = 0
			};
		}

		return match.BuildGameSetupResponse(request.PlayerId);
	}
}
