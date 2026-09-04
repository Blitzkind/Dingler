extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Client.Network.Escrow;
using HexGame::Game.Shared;
using HexGame::Game.Shared.Network.Escrow;

namespace Dingler.Game.Handlers.Request.Store;

[Authenticated]
public sealed class GetStoreItemsRequestHandler : IRequestHandler<GetStoreItemsRequestArgs, GetStoreItemsResponse>
{
	private readonly GetStoreItemsResponse _response = new()
	{
		StoreItems = new List<StoreItem>(),
	};
	public GetStoreItemsResponse HandleRequest(SessionContext context, GetStoreItemsRequestArgs request)
	{
		return _response;
	}
}