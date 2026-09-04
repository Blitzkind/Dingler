extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared.Profile;

namespace Dingler.Game.Handlers.Request.AuctionHouse;

[Authenticated]
public sealed class NetworkRequestHandler : IRequestHandler<Network.Request, Network.Response>
{
	private readonly Network.Response _response = new Network.Response()
	{
		Envelope = new byte[1]
	};
	
	public Network.Response HandleRequest(SessionContext context, Network.Request request)
	{
		return _response;
	}
}