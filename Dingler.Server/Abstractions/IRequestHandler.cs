namespace Dingler.Server.Abstractions;

public interface IRequestHandler<in TRequest>
{
	void HandleRequest(SessionContext context, TRequest request);
}

public interface IRequestHandler<in TRequest, out TResponse>
{
	TResponse HandleRequest(SessionContext context, TRequest request);
}