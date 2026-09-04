using System.Net.Sockets;
using Dingler.Server.Abstractions;
using Microsoft.Extensions.Logging;

namespace Dingler.Game.Protocol;

public sealed class HexStreamHandler : IStreamHandler
{
	private const string IDENTIFIER = "~HCP~";
	private readonly ILogger<HexStreamHandler>? _logger;

	public HexStreamHandler(ILogger<HexStreamHandler>? logger = null)
	{
		_logger = logger;
	}
	
	public async Task<byte[]> ReadAsync(NetworkStream stream, CancellationToken token)
	{
		int readTotal = 0;
		var lengthBuffer = new byte[IDENTIFIER.Length + 4];

		while (readTotal < lengthBuffer.Length)
		{
			int read = 0;

			try
			{
				read = await stream.ReadAsync(lengthBuffer.AsMemory(readTotal, lengthBuffer.Length - readTotal), token).ConfigureAwait(false);
			}
			catch (InvalidOperationException ex)
			{
				_logger?.LogError("Error reading client stream : {exceptionMessage}", ex.Message);
				throw;
			}

			if (read == 0)
			{
				throw new OperationCanceledException();
			}

			readTotal += read;
		}

		readTotal = 0;

		var fullMessageLength = BitConverter.ToUInt32(lengthBuffer.Skip(IDENTIFIER.Length).Take(4).Reverse().ToArray());

		byte[] messageBuffer = new byte[fullMessageLength];

		while (readTotal < fullMessageLength)
		{
			int read = await stream.ReadAsync(messageBuffer.AsMemory(readTotal, messageBuffer.Length - readTotal), token).ConfigureAwait(false);

			if (read == 0)
			{
				throw new OperationCanceledException();
			}

			readTotal += read;
		}

		return messageBuffer;
	}
}