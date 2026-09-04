extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared.Domain;
using HexGame::Game.Shared.Escrow.Messages;

namespace Dingler.Game.Handlers.Request.AuctionHouse;

[Authenticated]
public sealed class QueryAuctionItemsInfoRequestHandler : IRequestHandler<Auction.QueryAuctionItemsInfo.Request, Auction.QueryAuctionItemsInfo.Response>
{
	private readonly Auction.QueryAuctionItemsInfo.Response _response = new Auction.QueryAuctionItemsInfo.Response()
	{
		InventoryRes = new List<inventory_bits>(),
		ChestRes = new List<chest_bits>()
	};
	
	public Auction.QueryAuctionItemsInfo.Response HandleRequest(SessionContext context, Auction.QueryAuctionItemsInfo.Request request)
	{
		return _response;
	}
}