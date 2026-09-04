using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Game.Protocol.Messages.Args;
using Dingler.Game.Protocol.Messages.Requests;

namespace Dingler.Game.Handlers.Request.Session;

public sealed class PingRequestHandler : IRequestHandler<PingRequestArg, PingResponseArg>
{
	public PingRequestHandler()
	{ }

	public PingResponseArg HandleRequest(SessionContext context, PingRequestArg request)
	{
		return new PingResponseArg();
	}
}