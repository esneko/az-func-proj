using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using AzFuncProj.Storage.Service;

namespace AzFuncProj;

class Program
{
    static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(s =>
            {
                // s.AddApplicationInsightsTelemetryWorkerService();
                // s.ConfigureFunctionsApplicationInsights();
                s.AddHttpClient<TimerTrigger>();
                s.AddTransient<IStorageService, StorageService>();
            })
            .Build();

        await host.RunAsync();
    }
}
