using System.Reflection;
using System.Security.Authentication;
using System.Text;
using Dingler.Server.Abstractions;
using Dingler.Server.Attributes;

namespace Dingler.Server.Pipeline;

public sealed class HandlerMiddleware : IMiddleware<RequestContext>
{
	private readonly Dictionary<Type, Func<RequestContext, object, CancellationToken, Task>> _handlers = new();
	
	public void RegisterHandler<TRequest>(IAsyncRequestHandler<TRequest> handler)
	{
		var handlerType = handler.GetType();

		var isAuthRequired = handlerType.GetCustomAttribute<AuthenticatedAttribute>() is not null;
		
		_handlers[typeof(TRequest)] = (context, request, token) =>
		{
			var sessionContext = context.SessionContext;
			
			if (isAuthRequired && !sessionContext.IsAuthenticated)
				throw new AuthenticationException($"User is not authenticated.");

			return request is not TRequest typedRequest
				? throw new InvalidOperationException($"Invalid request type {typeof(TRequest).Name}")
				: handler.HandleRequestAsync(sessionContext, typedRequest, token);
		};
		
	}

	public void RegisterHandler<TRequest, TResponse>(IAsyncRequestHandler<TRequest, TResponse> handler)
	{
		var handlerType = handler.GetType();
		
		var isAuthRequired = handlerType.GetCustomAttribute<AuthenticatedAttribute>() is not null;
		
		_handlers[typeof(TRequest)] = async (context, request, token) =>
		{
			var sessionContext = context.SessionContext;
			
			if (isAuthRequired && !sessionContext.IsAuthenticated)
				throw new AuthenticationException($"User is not authenticated.");
			
			context.ResponseObject = request is not TRequest typedRequest
				? throw new InvalidOperationException($"Invalid request type {typeof(TRequest).Name}")
				: await handler.HandleRequestAsync(sessionContext, typedRequest, token);
		};
	}
	
	public void RegisterHandler<TRequest>(IRequestHandler<TRequest> handler)
	{
		var handlerType = handler.GetType();

		var isAuthRequired = handlerType.GetCustomAttribute<AuthenticatedAttribute>() is not null;
		
		_handlers[typeof(TRequest)] = (context, request, _) =>
		{
			var sessionContext = context.SessionContext;
			
			if (isAuthRequired && !sessionContext.IsAuthenticated)
				throw new AuthenticationException($"User is not authenticated.");

			if (request is not TRequest typedRequest)
				throw new InvalidOperationException($"Invalid request type {typeof(TRequest).Name}");
			
			handler.HandleRequest(sessionContext, typedRequest);
			return Task.CompletedTask;
		};
		
	}
	
	public void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
	{
		var handlerType = handler.GetType();
		
		var isAuthRequired = handlerType.GetCustomAttribute<AuthenticatedAttribute>() is not null;
		
		_handlers[typeof(TRequest)] = (context, request, _) =>
		{
			var sessionContext = context.SessionContext;
			
			if (isAuthRequired && !sessionContext.IsAuthenticated)
				throw new AuthenticationException($"User is not authenticated.");
			
			if (request is not TRequest typedRequest)
				throw new InvalidOperationException($"Invalid request type {typeof(TRequest).Name}");

			context.ResponseObject = handler.HandleRequest(sessionContext, typedRequest);

			return Task.CompletedTask;
		};
	}

	public async Task InvokeAsync(RequestContext context, PipelineBuilder<RequestContext>.MiddlewareDelegate next,
		CancellationToken token)
	{
		if (context.RequestObject is null || !_handlers.TryGetValue(context.RequestObject.GetType(),
			    out var handler))
		{
			var fullName = context.RequestObject?.GetType().FullName;

			if (fullName is null)
			{
				// assume json, print the whole thing.
				throw new InvalidOperationException(
					$"Unknown request type {Encoding.UTF8.GetString(context.RawRequest)}");
			}
			
			throw new InvalidOperationException(
				$"Unknown request type {fullName}");
		}

		await handler(context, context.RequestObject, token);
		await next(context, token);
	}
}