using System.Runtime.Loader;
using Dingler.Data.Context;
using Dingler.Game.CompositionRoot;
using Dingler.Game.Configuration;
using Dingler.Game.HarmonyPatches;
using Dingler.Game.Protocol;
using Dingler.Terminal.Frontend;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Dingler.Terminal;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        IHost? host = null;
        try
        {
            Console.Title = "Dingler.Terminal";
            
            // Don't touch this. It's what allows users to just drop the dlls into the directory
            // and it'll just work
            AssemblyLoadContext.Default.Resolving += (context, name) =>
            {
                var candidate = Path.Combine(AppContext.BaseDirectory, name.Name + ".dll");
                return File.Exists(candidate) ? context.LoadFromAssemblyPath(candidate) : null;
            };
            
            host = Host.CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .UseSerilog((context, services, config) =>
                {
                    config
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services);
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<AdminConsole>();
                })
                .BuildHex();

            await host.StartAsync();
            
            using (var scope = host.Services.CreateScope())
            {
                var adminContext = scope.ServiceProvider.GetRequiredService<AdminContext>();
                var gameContext = scope.ServiceProvider.GetRequiredService<HexGameContext>();
                var playerDataContext = scope.ServiceProvider.GetRequiredService<PlayerDataContext>();
                await adminContext.Database.MigrateAsync();
                await gameContext.Database.MigrateAsync();
                await playerDataContext.Database.MigrateAsync();

            }

            var staticLogger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Dingler");
            StaticLogger.SetLogger(staticLogger);
            
            UnityTypeResolver.Initialize();
            HarmonyPatcher.Patch();

            var adminConsole = host.Services.GetRequiredService<AdminConsole>();

            await adminConsole.RunAsync().ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            Log.Information("Application ended");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not start application: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            host?.Dispose();
        }
    }
}