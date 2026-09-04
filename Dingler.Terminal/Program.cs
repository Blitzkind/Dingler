using System.Runtime.Loader;
using Dingler.Data.Context;
using Dingler.Game;
using Dingler.Game.CompositionRoot;
using Dingler.Game.Configuration;
using Dingler.Game.HarmonyPatches;
using Dingler.Terminal.Frontend;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endPoints =>
                        {
                            //endPoints.MapServerEndpoints();
                        });
                    });
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<AdminConsole>();
                })
                .BuildHex();

            using (var scope = host.Services.CreateScope())
            {
                var gameDataContext = scope.ServiceProvider.GetRequiredService<GameDataContext>();

                await gameDataContext.Database.MigrateAsync();

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