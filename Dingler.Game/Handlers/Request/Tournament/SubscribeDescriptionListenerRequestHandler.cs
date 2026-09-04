extern alias HexGame;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Client.Network.Tournaments;
using HexGame::Game.Shared.Network.Tournaments;

namespace Dingler.Game.Handlers.Request.Tournament;

[Authenticated]
public sealed class SubscribeDescriptionListenerRequestHandler
	: IRequestHandler<SubscribeDescriptionListenerRequestArgs, SubscribeDescriptionListenerResponse>
{
	public SubscribeDescriptionListenerResponse HandleRequest(SessionContext context,
		SubscribeDescriptionListenerRequestArgs request)
	{
		return new SubscribeDescriptionListenerResponse()
		{
			Error = ESubscribeDescriptionListenerError.Ok,
			ErrorMessage = "Success"
		};
	}
}
