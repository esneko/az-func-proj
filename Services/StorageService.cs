using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzFuncProj.Storage.Models;

namespace AzFuncProj.Storage.Service;

public class StorageService : IStorageService
{
  private const string _connectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=ateatestrgbbc5;AccountKey=lmgXjRiHpymHg5lbdRtIy51ohHx9UC73LuB5xvzEQxIpyxL56B7E+8fhHyZig2T7uCVsaqRw0JrI+AStV2qJOQ==";
  private const string _containerName = "azfuncst";
  private const string _tableName = "azfunctb";

  public StorageService()
  {
  }

  public async Task<bool> UploadFile(string blobName, string blobData)
  {
    var storageClient = new BlobServiceClient(_connectionString);
    var containerClient = storageClient.GetBlobContainerClient(_containerName);
    var blobClient = containerClient.GetBlobClient(blobName);

    await blobClient.UploadAsync(BinaryData.FromString(blobData), overwrite: true);

    // using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobData)))
    // {
    //   await blobClient.UploadAsync(ms);
    // }

    return true;
  }

  public async Task<bool> AddEntities(string id, List<Entry> entries)
  {
    var tableClient = new TableClient(_connectionString, _tableName);

    foreach (var entry in entries)
    {
      entry.PartitionKey = "pk"; // DateTime.Now;
      entry.RowKey = id;

      await tableClient.AddEntityAsync(entry);
    }

    return true;
  }

  public async Task<IEnumerable<Entry>> ListEntities(string? filter = null)
  {
    var tableClient = new TableClient(_connectionString, _tableName);
    AsyncPageable<Entry> queryResults = tableClient.QueryAsync<Entry>(filter: filter, maxPerPage: 100);

    var results = new List<Entry>();
    await foreach (Page<Entry> page in queryResults.AsPages())
    {
      results.AddRange(page.Values);
    }

    return results.AsEnumerable();
  }
}
