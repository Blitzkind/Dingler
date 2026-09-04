namespace Dingler.Server.Abstractions;

public interface IAsyncRequestHandler<in TRequest>
{
	Task HandleRequestAsync(SessionContext context, TRequest request, CancellationToken token);
}

public interface IAsyncRequestHandler<in TRequest, TResponse>
{
	Task<TResponse> HandleRequestAsync(SessionContext context, TRequest request, CancellationToken token);
}