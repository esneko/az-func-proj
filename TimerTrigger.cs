using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
// using System.Net.Http.Json;
// using System.Text.Json;
using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Azure.Functions.Extensions; 
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AzFuncProj.Storage.Service;

namespace AzFuncProj;

public class TimerTrigger
{
    private readonly HttpClient _client;
    private readonly IStorageService _storage;

    public TimerTrigger(HttpClient httpClient, IStorageService storageService)
    {
        this._client = httpClient;
        this._storage = storageService;
    }

    [FunctionName("TimerTrigger")]
    public async Task Run(
        [TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
        ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        try
        {
            // var response = await _client.GetFromJsonAsync<Payload>("https://api.publicapis.org/random?auth=null");
            var response = await _client.GetAsync("https://api.publicapis.org/random?auth=null");
            var payload = await response.Content.ReadAsStringAsync();
            bool ok = _storage.SaveFile(payload);

            // Payload payload = JsonSerializer.Deserialize<Payload>(payload);
            // if (payload?.count > 0)
            //     _storage.SaveData(payload.entries);

            if (ok) log.LogInformation($"C# Timer trigger function called the API: {payload}");
        }
        catch (HttpRequestException ex)
        {
            log.LogInformation($"C# Timer trigger function failed to call the API: {ex.Message}");
            throw;
        }
    }
}
