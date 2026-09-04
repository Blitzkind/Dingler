using Dingler.Server.Abstractions;

namespace Dingler.Server;

public sealed class CancellationManager : ICancellationManager
{
	private CancellationTokenSource _cts = new();

	public CancellationToken StoppingToken => _cts.Token;
	public CancellationTokenSource CreateLinkedSource()
	{
		return CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
	}

	public void RefreshToken()
	{
		_cts.Dispose();
		_cts = new CancellationTokenSource();
	}

	public void Stop() => _cts.Cancel();

	public void Dispose()
	{
		if (!_cts.IsCancellationRequested)
			_cts.Cancel();
		_cts.Dispose();
	}
}