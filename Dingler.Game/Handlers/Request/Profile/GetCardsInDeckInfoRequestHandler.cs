extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Client.Network.Profile;
using HexGame::Game.Shared.Network.Profile;

namespace Dingler.Game.Handlers.Request.Profile;

[Authenticated]
public sealed class GetCardsInDeckInfoRequestHandler : IRequestHandler<GetCardsInDeckInfoRequestArgs, GetCardsInDeckInfoResponse>
{
	public GetCardsInDeckInfoResponse HandleRequest(SessionContext context, GetCardsInDeckInfoRequestArgs request)
	{
		return new GetCardsInDeckInfoResponse()
		{
			cardsDeckAndLockInfos = new()
		};
	}
}