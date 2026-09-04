extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Client.Network.Tournaments;
using  HexGame::Game.Shared.Network.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;


[Authenticated]
public sealed class UnsubscribeDescriptionListenerRequestHandler
	: IRequestHandler<UnsubscribeDescriptionListenerRequestArgs, UnsubscribeDescriptionListenerResponse>
{
	public UnsubscribeDescriptionListenerResponse HandleRequest(SessionContext context,
		UnsubscribeDescriptionListenerRequestArgs request)
	{
		var response = new UnsubscribeDescriptionListenerResponse()
		{
			Error = EUnsubscribeDescriptionListenerError.Ok
		};

		return response;
	}
}