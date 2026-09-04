extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared.Network.Campaign;

namespace Dingler.Game.Handlers.Request.Campaign;

[Authenticated]
public sealed class GetMasterListOfChallengersRequestHandler : IRequestHandler<GetMasterListOfChallengersRequestArgs>
{
	public void HandleRequest(SessionContext context, GetMasterListOfChallengersRequestArgs request)
	{
		
	}
}