using System.Net.Sockets;
using Dingler.Server.Abstractions;
using Dingler.Server.Pipeline;
using Microsoft.Extensions.Logging;

namespace Dingler.Server
{
    public sealed class DinglerServer : IDinglerServer, IDisposable
    {
        private readonly TcpListener _tcpListener;
        private readonly SessionManager _sessionManager;
        private readonly IStreamHandler _streamHandler;
        private readonly IEnumerable<IStartupService> _startupServices;
        private readonly IEnumerable<IAsyncStartupService> _asyncStartupServices;
        private readonly ICancellationManager _cancellationManager;
        private Task? _listenTask;
        private bool _disposedValue;
        private readonly ILoggerFactory? _loggerFactory;
        private readonly ILogger<DinglerServer>? _logger;

        private readonly IncomingPipeline _incomingPipeline;
        private readonly OutgoingPipeline _outgoingPipeline;
        
        public bool IsRunning { get; private set; }
        public DinglerServer(TcpListener tcpListener,
            SessionManager sessionManager,
            IStreamHandler streamHandler,
            IncomingPipeline incomingPipeline,
            OutgoingPipeline outgoingPipeLine,
            ICancellationManager cancellationManager,
            IEnumerable<IStartupService> startupServices,
            IEnumerable<IAsyncStartupService> asyncStartupServices,
            ILoggerFactory? loggerFactory)
        {
            _tcpListener = tcpListener;
            _sessionManager = sessionManager;
            _streamHandler = streamHandler;
            _cancellationManager = cancellationManager;
            _incomingPipeline = incomingPipeline;
            _outgoingPipeline = outgoingPipeLine;
            _startupServices = startupServices;
            _asyncStartupServices = asyncStartupServices;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory?.CreateLogger<DinglerServer>();
        }

        public async Task StartAsync()
        {

            _logger?.LogInformation("Starting server");

            _cancellationManager.RefreshToken();
            try
            {
                if (IsRunning)
                    return;
                var startupTasks = _asyncStartupServices.Select(asyncService =>
                        asyncService.InitializeAsync(_cancellationManager.StoppingToken)).ToList();

                foreach (var service in _startupServices)
                {
                    service.Initialize();
                }

                await Task.WhenAll(startupTasks);
                
                _tcpListener.Start();
                _logger?.LogInformation("Server started");
                _listenTask = ListenForClientsAsync(_cancellationManager.StoppingToken);
            }
            catch (Exception e)
            {
                _logger?.LogCritical(e, "Server failed");
                await StopAsync().ConfigureAwait(false);
                return;
            }
            
            IsRunning = true;
        }

        public async Task StopAsync()
        {
            if (!IsRunning)
                return;
            
            _logger?.LogInformation("Stopping server");

            _tcpListener.Stop();
            _cancellationManager.Stop();
            
            await _listenTask!.ConfigureAwait(false);

            IsRunning = false;
            
            _logger?.LogInformation("Server stopped");
        }

        private async Task ListenForClientsAsync(CancellationToken token)
        {
            _logger?.LogInformation("Listening for clients on {localEndpoint}", _tcpListener.LocalEndpoint);

            List<Task> handleTasks = new();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync(token).ConfigureAwait(false);

                    _logger?.LogInformation("Client {clientId} connected", tcpClient.Client.RemoteEndPoint);

                    if (!_sessionManager.TryCreateSession(out var context))
                    {
                        _logger?.LogError("Could not create session for {clientId}", tcpClient.Client.RemoteEndPoint);
                        continue;
                    }

                    var client = new DinglerClient(context, tcpClient, _incomingPipeline, _outgoingPipeline,
                        _streamHandler, _loggerFactory?.CreateLogger<DinglerClient>());

                    handleTasks.Add(HandleClientRequests(client));
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (SocketException) when (token.IsCancellationRequested)
            {
            }
            catch (Exception e)
            {
                _logger?.LogCritical(e, "Server failed");
                await StopAsync().ConfigureAwait(false);
            }
            finally
            {
                await Task.WhenAll(handleTasks);
            }
        }
        
        private async Task HandleClientRequests(DinglerClient client)
        {
            try
            {
                await client.RunAsync(_cancellationManager)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                _logger?.LogInformation("Session Id {id} has logged off.", client.SessionContext.SessionId);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Handle clients for {id} failed miserably!", client.SessionContext.SessionId);
            }
            finally
            {
                _sessionManager.TryRemoveSession(client.SessionContext);
                client.Dispose();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _cancellationManager.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }
    }
}
