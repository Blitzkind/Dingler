using System.Net;
using Dingler.Server.Pipeline;

namespace Dingler.Server
{
    public sealed class ServerConfiguration
    {
        public IPAddress Url { get; set; } = IPAddress.Any;
        public int Port { get; set; } = 9933;
        public int IdleTimeoutSeconds { get; set; } = 120;
        public PipelineBuilder<RequestContext> IncomingPipelineBuilder { get; set; } = new();
        public PipelineBuilder<RequestContext> OutgoingPipelineBuilder { get; set; } = new();
    }
}
