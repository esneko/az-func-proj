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

namespace az_func_proj;

public class Entry
{
    public string API;
    public string Description;
    public string Auth;
    public bool HTTPS;
    public string Cors;
    public string Link;
    public string Category;
}

public class Payload
{
    public int count;
    public List<Entry> entries;
}

public class TimerTrigger
{
    private readonly HttpClient _client;

    public TimerTrigger(HttpClient httpClient) //, IStorageService storage
    {
        this._client = httpClient;
        // this._storage = storage;
    }

    [FunctionName("TimerTrigger")]
    public async Task Run(
        [TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
        ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        try
        {
            var response = await _client.GetFromJsonAsync<Payload>("https://api.publicapis.org/random?auth=null");
            log.LogInformation($"C# Timer trigger function called the API: {response.count}");
        }
        catch (HttpRequestException ex)
        {
            log.LogInformation($"C# Timer trigger function failed to call the API: {ex.Message}");
            throw;
        }
    }
}
