namespace Dingler.Server.Pipeline;

public sealed class IncomingPipeline
{
	public PipelineBuilder<RequestContext>.MiddlewareDelegate Delegate { get; }

	public IncomingPipeline(PipelineBuilder<RequestContext>.MiddlewareDelegate del)
	{
		Delegate = del;
	}
}