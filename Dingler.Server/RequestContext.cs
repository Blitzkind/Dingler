using Dingler.Server.Systems;

namespace Dingler.Server;

public sealed class RequestContext : MessageContext
{
	public byte[] RawRequest { get; set; }
	public object? RequestObject { get; set; }
	public byte[]? RawResponse { get; set; }
	public object? ResponseObject { get; set; }
	public bool HasResponse => ResponseObject is not null;
	public SessionContext SessionContext { get; }
	
	
	public RequestContext(byte[] rawRequest, SessionContext sessionContext)
	{
		RawRequest = rawRequest;
		SessionContext = sessionContext;
	}
}