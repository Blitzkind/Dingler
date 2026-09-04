namespace Dingler.Server.Abstractions;

public interface IAsyncStartupService
{
	Task InitializeAsync(CancellationToken token);
}