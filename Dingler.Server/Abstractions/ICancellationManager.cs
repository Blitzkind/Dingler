namespace Dingler.Server.Abstractions;

public interface ICancellationManager : IDisposable
{
	CancellationToken StoppingToken { get; }
	CancellationTokenSource CreateLinkedSource();
	void Stop();
	void RefreshToken();
}