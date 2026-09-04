extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared.Network.Profile;

namespace Dingler.Game.Handlers.Request.Profile;

[Authenticated]
public sealed class AddChampionRequestHandler : IRequestHandler<AddChampionRequestArgs>
{
	public void HandleRequest(SessionContext context, AddChampionRequestArgs request)
	{
		
	}
}