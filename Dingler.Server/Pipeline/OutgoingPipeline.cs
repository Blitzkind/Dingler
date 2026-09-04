namespace Dingler.Server.Pipeline;

public sealed class OutgoingPipeline
{
	public PipelineBuilder<RequestContext>.MiddlewareDelegate Delegate { get; }

	public OutgoingPipeline(PipelineBuilder<RequestContext>.MiddlewareDelegate del)
	{
		Delegate = del;
	}
}