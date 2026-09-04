namespace Dingler.Server.Abstractions
{
    public interface IDinglerServer
    {
        Task StartAsync();
        Task StopAsync();
        bool IsRunning { get; }

    }
}
