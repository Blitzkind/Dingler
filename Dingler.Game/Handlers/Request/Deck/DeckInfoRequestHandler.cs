extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using Dingler.Game.Services;
using HexGame::Game.Client.Network.Profile;
using HexGame::Game.Shared.Network.Profile;

namespace Dingler.Game.Handlers.Request.Deck;

[Authenticated]
public sealed class DeckInfoRequestHandler : IRequestHandler<GetDeckInfoRequestArgs, GetDeckInfoResponse>
{
	private readonly DeckService _deckService;
	
	public DeckInfoRequestHandler(DeckService deckService)
	{
		_deckService = deckService;
	}

	public GetDeckInfoResponse HandleRequest(SessionContext context, GetDeckInfoRequestArgs request)
	{
		try
		{
			return _deckService.GetDeckInfo(context, request);
		}
		catch (InvalidOperationException)
		{
			return new GetDeckInfoResponse()
			{
				Error = EGetDeckInfoError.InternalServerError,
				ErrorMessage = "Could not retrieve deck information."
			};
		}
	}
}