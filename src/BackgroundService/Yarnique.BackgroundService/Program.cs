using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Yarnique.BackgroundService.Configuration;
using Yarnique.BackgroundService.Helpers;

namespace Yarnique.BackgroundService
{
    public class Program
    {
        private static BackgroundServiceConfig _config;

        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()                
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddCommandLine(args);
                    var builtConfig = config.Build();

                    _config = new BackgroundServiceConfig();
                    builtConfig.Bind(_config);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.SetMinimumLevel(LogLevel.Debug);
                    builder.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<BackgroundServiceConfig>(_config);
                    services.AddSingleton<IProcessOrderService, ProcessOrderService>();
                    services.AddHostedService<OrderExecutionWatcherService>();
                })
                .UseConsoleLifetime();

            using var host = builder.Build();
            await host.RunAsync();
        }
    }
}
