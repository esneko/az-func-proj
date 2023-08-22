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

  public async Task<bool> AddEntities(string id, List<Entry> entities)
  {
    var tableClient = new TableClient(_connectionString, _tableName);

    List<TableTransactionAction> addEntityBatch = new List<TableTransactionAction>();

    addEntityBatch.AddRange(entities.Select(f => new TableTransactionAction(TableTransactionActionType.Add, new Entry
    {
      PartitionKey = DateOnly.FromDateTime(DateTime.Now).ToString("O"), // DateTimeOffset.UtcNow.ToUnixTimeSeconds()
      RowKey = id,
      API = "public",
      Link = "www",
      Description = "test",
      Category = "abc",
      Auth = "none",
      Cors = "*",
      HTTPS = true
    })));

    Response<IReadOnlyList<Response>> response = await tableClient.SubmitTransactionAsync(addEntityBatch);

    return true;
  }

  public async Task<List<T>> ListEntities<T>(string? filter = null) where T : class, ITableEntity
  {
    var tableClient = new TableClient(_connectionString, _tableName);
    AsyncPageable<T> queryResults = tableClient.QueryAsync<T>(filter: filter, maxPerPage: 100);

    var results = new List<T>();
    await foreach (Page<T> page in queryResults.AsPages())
    {
      results.AddRange(page.Values);
    }

    return results;
  }
}
