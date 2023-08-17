using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using AzFuncProj.Storage.Models;

namespace AzFuncProj.Storage.Service;

public class StorageService : IStorageService
{
  private string _connectionString;
  private BlobServiceClient _client;

  public StorageService()
  {
    // TODO: read from Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_CONNECTION_STRING")
    _connectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=ateatestrgbbc5;AccountKey=lmgXjRiHpymHg5lbdRtIy51ohHx9UC73LuB5xvzEQxIpyxL56B7E+8fhHyZig2T7uCVsaqRw0JrI+AStV2qJOQ==";
    _client = new BlobServiceClient(_connectionString);
  }

  public async Task<bool> SaveFile(string blobName, string blobData)
  {
    BlobContainerClient containerClient = _client.GetBlobContainerClient("azfuncst");
    BlobClient blobClient = containerClient.GetBlobClient(blobName);

    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
    {
      await blobClient.UploadAsync(ms);
    }

    // await blobClient.UploadAsync(BinaryData.FromString(blobData), overwrite: true);

    return true;
  }

  public bool SaveData(List<Entry> entries)
  {
    return true;
  }
}
