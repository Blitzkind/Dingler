using Dingler.Server.Pipeline;

namespace Dingler.Server
{
    public sealed class ServerConfiguration
    {
        public string Url { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 9933;
        public PipelineBuilder<RequestContext> IncomingPipelineBuilder { get; set; } = new();
        public PipelineBuilder<RequestContext> OutgoingPipelineBuilder { get; set; } = new();
    }
}
