using System.Net.Sockets;

namespace Dingler.Server.Abstractions;

public interface IStreamHandler
{
	Task<byte[]> ReadAsync(NetworkStream stream, CancellationToken token);
}