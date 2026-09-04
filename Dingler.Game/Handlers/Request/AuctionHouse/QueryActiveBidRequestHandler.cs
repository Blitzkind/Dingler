extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared.Escrow.Messages;

namespace Dingler.Game.Handlers.Request.AuctionHouse;

[Authenticated]
public sealed class QueryActiveBidRequestHandler : IRequestHandler<Auction.QueryActiveBid.Request, Auction.QueryActiveBid.Response>
{
	private readonly Auction.QueryActiveBid.Response _response = new()
	{
		CardRes = null,
		IntentoryRes = null,
	};
	public Auction.QueryActiveBid.Response HandleRequest(SessionContext context, Auction.QueryActiveBid.Request request)
	{
		return _response;
	}
}