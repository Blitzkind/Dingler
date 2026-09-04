using Dingler.Server.Abstractions;

namespace Dingler.Server.Pipeline;

public sealed class PipelineBuilder<TContext>
{
	public delegate Task MiddlewareDelegate(TContext context, CancellationToken token);
	public delegate Task Middleware(TContext context, MiddlewareDelegate next, CancellationToken token);
	
	private readonly List<Middleware> _middlewares = new();

	public PipelineBuilder<TContext> Use(Middleware middleware)
	{
		_middlewares.Add(middleware);
		return this;
	}

	public PipelineBuilder<TContext> Use(IMiddleware<TContext> middleware)
	{
		_middlewares.Add(middleware.InvokeAsync);
		return this;
	}

	internal MiddlewareDelegate Build()
	{
		MiddlewareDelegate pipeline = (_, token) => Task.CompletedTask;

		for (int i = _middlewares.Count - 1; i >= 0; i--)
		{
			var middleware = _middlewares[i];
			var next = pipeline;
			pipeline = (context, token) => middleware(context, next, token);
		}
		
		return pipeline;
	}
}