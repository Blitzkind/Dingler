extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Escrow.Messages;

namespace Dingler.Game.Handlers.Request.AuctionHouse;

[Authenticated]
public sealed class QueryAuctionActiveTemplatesRequestHandler : IRequestHandler<Auction.QueryAuctionActiveTemplates.Request, Auction.QueryAuctionActiveTemplates.Response>
{
	private readonly Auction.QueryAuctionActiveTemplates.Response _response = new Auction.QueryAuctionActiveTemplates.Response()
	{
		BidCardT = new List<ResourceId>(),
		BidInvenT = new List<ResourceId>(),
		SellCardT = new List<ResourceId>(),
		SellInvenT = new List<ResourceId>()
	};
	
	public Auction.QueryAuctionActiveTemplates.Response HandleRequest(SessionContext context, Auction.QueryAuctionActiveTemplates.Request request)
	{
		return _response;
	}
}