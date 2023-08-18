using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AzFuncProj.Storage.Service;
using AzFuncProj.Storage.Models;

namespace AzFuncProj;

public class TimerTrigger
{
    private HttpClient _client;
    private IStorageService _storage;

    public TimerTrigger(HttpClient httpClient, IStorageService storageService)
    {
        _client = httpClient;
        _storage = storageService;
    }

    [FunctionName("TimerTrigger")]
    public async Task Run(
        [TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
        ILogger log)
    {
        log.LogInformation($"C# TimerTrigger function executed at: {DateTime.Now}");
        try
        {
            var response = await _client.GetAsync("https://api.publicapis.org/random?auth=null");
            var data = await response.Content.ReadAsStringAsync();

            string id = Guid.NewGuid().ToString();
            await _storage.UploadFile(id, data);

            var payload = JsonSerializer.Deserialize<Payload>(data);
            if (payload.count > 0)
            {
                _storage.AddEntities(id, payload.entries);
            }

            log.LogInformation($"C# TimerTrigger function called the API: {data}");
        }
        catch (HttpRequestException ex)
        {
            log.LogInformation($"Error: {ex.Message}");
            throw;
        }
        catch (NotSupportedException)
        {
            log.LogInformation($"Error: The content type is not supported.");
            throw;
        }
        catch (JsonException)
        {
            log.LogInformation($"Error: Invalid JSON.");
            throw;
        }
    }
}
