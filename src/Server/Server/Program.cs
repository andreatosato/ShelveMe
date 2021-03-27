using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Services;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Server
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()                
                .ConfigureServices(services =>
                {
                    services.AddOptions<TableStorageOptions>()
                        .Configure<IConfiguration>((settings, configuration) =>
                        {
                            configuration.GetSection("TableStorage").Bind(settings);
                        });
                    services.AddSingleton<ITableStorageService, TableStorageService>(sp =>
                        new TableStorageService(sp.GetService<ILogger<TableStorageService>>(), sp.GetService<IOptions<TableStorageOptions>>()));
                })
                .ConfigureFunctionsWorkerDefaults()
                .Build();


            host.Run();

            // func host start --verbose
        }
    }
}
