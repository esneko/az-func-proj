using System;
using System.Collections.Generic;
using System.IO;
// using System.Net.Http;
// using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Azure.Functions.Extensions; 
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AzFuncProj.Storage.Service;
// using AzFuncProj.Storage.Models;

namespace AzFuncProj;

public class TimerTrigger
{
    private readonly IHttpClientFactory _client;
    private readonly IStorageService _storage;

    public TimerTrigger(IHttpClientFactory httpClient, IStorageService storageService)
    {
        this._httpClient = httpClient;
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
            var client = _httpClient.CreateClient("TestClientController");
            // var response = await _client.GetFromJsonAsync<Payload>("https://api.publicapis.org/random?auth=null");
            var response = await client.GetAsync("https://api.publicapis.org/random?auth=null");
            var payload = await response.Content.ReadAsStringAsync();
            _storage.SaveFile(payload);

            // Payload payload = JsonSerializer.Deserialize<Payload>(payload);
            // if (payload?.count > 0)
            //     _storage.SaveData(payload.entries);

            log.LogInformation($"C# Timer trigger function called the API: {payload}");
        }
        catch (HttpRequestException ex)
        {
            log.LogInformation($"C# Timer trigger function failed: {ex.Message}");
            throw;
        }
        catch (NotSupportedException)
        {
            log.LogInformation($"C# Timer trigger function failed: The content type is not supported.");
            throw;
        }
        catch (JsonException)
        {
            log.LogInformation($"C# Timer trigger function failed: Invalid JSON.");
            throw;
        }
    }
}
