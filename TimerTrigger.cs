using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AzFuncProj.Storage.Service;
using AzFuncProj.Storage.Models;

namespace AzFuncProj;

public class TimerTrigger
{
    private readonly HttpClient _client;
    private readonly IStorageService _storage;
    private readonly ILogger _logger;

    public TimerTrigger(HttpClient httpClient, IStorageService storageService, ILoggerFactory loggerFactory)
    {
        _client = httpClient;
        _storage = storageService;
        _logger = loggerFactory.CreateLogger<TimerTrigger>();
    }

    [Function(nameof(TimerTrigger))]
    public async Task Run(
        [TimerTrigger("0 */1 * * * *")] MyInfo myTimer)
    {
        _logger.LogInformation($"C# TimerTrigger function executed at: {DateTime.Now}");
        _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
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

            _logger.LogInformation($"C# TimerTrigger function called the API: {payload.count} {payload.entries}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogInformation($"Error: {ex.Message}");
            throw;
        }
        catch (NotSupportedException)
        {
            _logger.LogInformation($"Error: The content type is not supported.");
            throw;
        }
        catch (JsonException)
        {
            _logger.LogInformation($"Error: Invalid JSON.");
            throw;
        }
    }
}

public class MyInfo
{
    public MyScheduleStatus ScheduleStatus { get; set; }

    public bool IsPastDue { get; set; }
}

public class MyScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}
