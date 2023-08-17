using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using AzFuncProj;
using AzFuncProj.Storage.Service;

[assembly: FunctionsStartup(typeof(Startup))]

namespace AzFuncProj;

public class Startup : FunctionsStartup
{
  public override void Configure(IFunctionsHostBuilder builder)
  {
    builder.Services.AddHttpClient<TimerTrigger>(c => c.BaseAddress = new Uri("https://api.publicapis.org/"));
    builder.Services.AddTransient<IStorageService, StorageService>();
  }
}
