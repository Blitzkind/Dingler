extern alias HexGame;
using System.Text.Json;
using Dingler.Server;
using Dingler.Server.Abstractions;
using Dingler.Server.Pipeline;
using Dingler.Game.Protocol.Messages;

namespace Dingler.Game.Protocol.Middleware;

extern alias HexGame;

public sealed class ParseMiddleware : IMiddleware<RequestContext>
{
	public async Task InvokeAsync(RequestContext context, PipelineBuilder<RequestContext>.MiddlewareDelegate next,
		CancellationToken token)
	{
		var parsedData = ParseData(context.RawRequest);
		var header = DeserializeHeader(parsedData.headerBytes);
		context.SetHeader(header);
		context.SetRequestId(header.RequestId);
		context.RawRequest = parsedData.payloadBytes;
		await next(context, token);
	}

	private (byte[] headerBytes, byte[] payloadBytes) ParseData(ReadOnlySpan<byte> data)
	{
		int currentByte = 0;
		var headerLength = GetLengthValue(data, currentByte);

		currentByte += 4;
		
		var headerBytes = data.Slice(currentByte, headerLength).ToArray();
		currentByte += headerLength;
		
		var bodyLength = GetLengthValue(data, currentByte);
		currentByte += 4;
		
		var bodyBytes = data.Slice(currentByte, bodyLength).ToArray();

		return (headerBytes, bodyBytes);
	}

	private Header DeserializeHeader(ReadOnlySpan<byte> headerBytes)
	{
		var header = JsonSerializer.Deserialize<Header>(headerBytes);
		
		if (header is null)
			throw new Exception("Invalid header");
		
		return header;
	}
	
	private int GetLengthValue(ReadOnlySpan<byte> buffer, int offset = 0)
	{
		return (buffer[offset + 0] << 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | buffer[offset + 3];
	}
}