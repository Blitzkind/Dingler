using Dingler.Server.Pipeline;

namespace Dingler.Server.Abstractions;

public interface IMiddleware<TContext>
{
	public Task InvokeAsync(TContext context, PipelineBuilder<TContext>.MiddlewareDelegate next,
		CancellationToken token);
}