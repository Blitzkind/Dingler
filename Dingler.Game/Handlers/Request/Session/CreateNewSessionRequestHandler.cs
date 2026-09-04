using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Game.Protocol.Messages.Args;

namespace Dingler.Game.Handlers.Request.Session;

public sealed class CreateNewSessionRequestHandler : IRequestHandler<SessionCreationRequestEvent, CreateNewSessionResponse>
{
	
	public CreateNewSessionRequestHandler()
	{ }

	public CreateNewSessionResponse HandleRequest(SessionContext context,
		SessionCreationRequestEvent request)
	{
		return new CreateNewSessionResponse();
	}
}