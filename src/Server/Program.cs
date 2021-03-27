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
        public static Task Main()
        {
            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddLogging();
                    services.AddOptions<TableStorageOptions>()
                        .Configure<IConfiguration>((settings, configuration) =>
                        {
                            settings.TableName = Environment.GetEnvironmentVariable("TableStorage:TableName");
                            settings.StorageAccount = Environment.GetEnvironmentVariable("TableStorage:StorageAccount");
                        });
                    services.AddSingleton<ITableStorageService, TableStorageService>(sp =>
                        new TableStorageService(sp.GetService<ILogger<TableStorageService>>(), sp.GetService<IOptions<TableStorageOptions>>()));
                })
                .ConfigureFunctionsWorkerDefaults()
                .Build();


            return host.RunAsync();

            // func host start --verbose
            // https://techcommunity.microsoft.com/t5/apps-on-azure/preview-creating-azure-functions-using-net-5/ba-p/2156846
        }
    }
}
