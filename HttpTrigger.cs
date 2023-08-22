using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

public class HttpTrigger
{
    private IStorageService _storage;
    private readonly ILogger _logger;

    public HttpTrigger(IStorageService storageService, ILoggerFactory loggerFactory)
    {
        _storage = storageService;
        _logger = loggerFactory.CreateLogger<TimerTrigger>();
    }

    [Function(nameof(HttpTrigger))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req)
    {
        _logger.LogInformation($"C# HttpTrigger function executed at: {DateTime.Now}");

        var response = req.CreateResponse(HttpStatusCode.OK);

        string partitionKey = req.Query["pk"];
        if (!string.IsNullOrEmpty(partitionKey))
        {
            var entries = await _storage.ListEntities<Entry>($"PartitionKey eq '{partitionKey}'");
            var data = JsonSerializer.Serialize<List<Entry>>(entries);

            response.Headers.Add("Content-Type", "text/json; charset=utf-8");
            response.WriteString(data);

            return response;
        }

        string id = req.Query["id"];

        response.Headers.Add("Content-Type", "text/html; charset=utf-8");
        response.WriteString($"Todo: fetch the blob with id: {id}");

        return response;
    }
}
