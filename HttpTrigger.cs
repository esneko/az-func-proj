using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AzFuncProj.Storage.Service;
using AzFuncProj.Storage.Models;

namespace AzFuncProj;

public class HttpTrigger
{
    private IStorageService _storage;

    public HttpTrigger(IStorageService storageService)
    {
        _storage = storageService;
    }

    [FunctionName("HttpTrigger")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation($"C# HttpTrigger function executed at: {DateTime.Now}");

        string partitionKey = req.Query["pk"];
        if (!string.IsNullOrEmpty(partitionKey))
        {
            var entities = await _storage.ListEntities($"PartitionKey eq '{partitionKey}'");
            var entries = JsonSerializer.Serialize<List<Entry>>(entities.ToList());

            return new OkObjectResult(entries);
        }

        string id = req.Query["id"];

        return new OkObjectResult($"Todo: fetch the blob with id: {id}");
    }
}
