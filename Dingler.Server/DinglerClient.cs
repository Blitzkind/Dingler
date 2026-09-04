using System.Net.Sockets;
using System.Threading.Channels;
using Dingler.Server.Abstractions;
using Dingler.Server.Pipeline;
using Microsoft.Extensions.Logging;

namespace Dingler.Server;

public sealed class DinglerClient : IDisposable
{
	private readonly TcpClient _client;
	private readonly IncomingPipeline _incomingPipeline;
	private readonly OutgoingPipeline _outgoingPipeline;
	private readonly IStreamHandler _streamHandler;
	private readonly Channel<byte[]> _incomingChannel;
	private readonly Channel<RequestContext> _outgoingChannel;
	private readonly ILogger<DinglerClient>? _logger;
	public SessionContext SessionContext { get; }

	
	public DinglerClient(SessionContext sessionContext, TcpClient client,
		IncomingPipeline incomingPipeline, 
		OutgoingPipeline outgoingPipeline,
		IStreamHandler streamHandler,
		ILogger<DinglerClient>? logger = null)
	{
		SessionContext = sessionContext;
		_client = client;
		_incomingPipeline = incomingPipeline;
		_outgoingPipeline = outgoingPipeline;
		_streamHandler = streamHandler;
		_logger = logger;
		_incomingChannel = Channel.CreateUnbounded<byte[]>();
		_outgoingChannel = Channel.CreateUnbounded<RequestContext>();
		sessionContext.SendMessageAsync += SendMessageAsync;
		sessionContext.TrySendMessage += TrySendMessage;
	}

	public async Task RunAsync(ICancellationManager cancellationManager)
	{
		using var cts = cancellationManager.CreateLinkedSource();
		var token = cts.Token;
		var stream = _client.GetStream();

		var tasks = new[]
		{
			ReadFromStreamAsync(stream, token),
			WriteToStreamAsync(stream, token),
			HandleRequestsAsync(token)
		};

		try
		{
			await Task.WhenAny(tasks);
		}
		finally
		{
			try
			{
				await cts.CancelAsync();
				await Task.WhenAll(tasks);
			}
			catch (ObjectDisposedException)
			{ }
			catch (OperationCanceledException)
			{ }
		}
	}

	private async Task SendMessageAsync(object message, CancellationToken token)
	{
		var context = new RequestContext(new byte[1], SessionContext)
		{
			ResponseObject = message
		};

		await _outgoingChannel.Writer.WriteAsync(context, token)
			.ConfigureAwait(false);
	}

	private bool TrySendMessage(object message)
	{
		var context = new RequestContext(new byte[1], SessionContext)
		{
			ResponseObject = message
		};
		
		return _outgoingChannel.Writer.TryWrite(context);
	}

	private async Task ReadFromStreamAsync(NetworkStream stream, CancellationToken token)
	{
		try
		{
			while (!token.IsCancellationRequested)
			{
				var data = await _streamHandler.ReadAsync(stream, token);
				await _incomingChannel.Writer.WriteAsync(data, token);
			}
		}
		finally
		{
			_incomingChannel.Writer.Complete();
		}
	}

	private async Task WriteToStreamAsync(Stream stream, CancellationToken token)
	{
		try
		{
			await foreach (var message in _outgoingChannel.Reader.ReadAllAsync(token))
			{
				try
				{
					await _outgoingPipeline.Delegate(message, token);
				}
				catch (NotImplementedException e)
				{
					_logger?.LogInformation(
						"Unknown message type received: {exception}",
						e.Message);
					continue;
				}

				if (message.RawResponse is null)
					throw new Exception("Response is null");
				
				await stream.WriteAsync(message.RawResponse, token);
			}
		}
		finally
		{
			_outgoingChannel.Writer.Complete();
		}
	}

	private async Task HandleRequestsAsync(CancellationToken token)
	{
		await foreach (var request in _incomingChannel.Reader.ReadAllAsync(token))
		{
			var requestContext = new RequestContext(request, SessionContext);
			try
			{
				await _incomingPipeline.Delegate(requestContext, token);

				if (requestContext.HasResponse)
				{
					await _outgoingChannel.Writer.WriteAsync(requestContext, token);
				}
			}
			catch (InvalidOperationException e)
			{
				_logger?.LogInformation(
					"Unknown message type received: {exception}",
					e.Message);
			}
		}
	}

	public void Dispose()
	{
		_client.Dispose();
		SessionContext.SendMessageAsync -= SendMessageAsync;
		SessionContext.TrySendMessage -= TrySendMessage;
		SessionContext.NotifyDisconnected();
	}
}