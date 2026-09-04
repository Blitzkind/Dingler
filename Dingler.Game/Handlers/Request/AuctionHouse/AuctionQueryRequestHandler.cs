extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared.Escrow.Messages;

namespace Dingler.Game.Handlers.Request.AuctionHouse;

[Authenticated]
public sealed class AuctionQueryRequestHandler : IRequestHandler<Auction.Query.Request, Auction.Query.Response>
{
	private readonly Auction.Query.Response _response = new Auction.Query.Response()
	{
		CardRes = null,
		InvenRes = null
	};
	public Auction.Query.Response HandleRequest(SessionContext context, Auction.Query.Request request)
	{
		return _response;
	}
}