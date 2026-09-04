extern alias HexGame; 
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;
using HexGame::Game.Shared.Campaign.Siege;

namespace Dingler.Game.Handlers.Request.Profile;
// We're ignoring this and throwing it away. It's a siege thing. It'll be implemented in version 2

[Authenticated]
public sealed class MessagingAsyncRequestHandler : IRequestHandler<Messaging.Request>
{
	public void HandleRequest(SessionContext context, Messaging.Request request)
	{
		
	}
}