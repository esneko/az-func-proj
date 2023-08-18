using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzFuncProj.Storage.Models;

namespace AzFuncProj.Storage.Service;

public class StorageService : IStorageService
{
  private string _connectionString;
  private string _containerName;
  private BlobServiceClient _client;

  public StorageService()
  {
    _connectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=ateatestrgbbc5;AccountKey=lmgXjRiHpymHg5lbdRtIy51ohHx9UC73LuB5xvzEQxIpyxL56B7E+8fhHyZig2T7uCVsaqRw0JrI+AStV2qJOQ==";
    _containerName = "azfuncst"
    _client = new BlobServiceClient(_connectionString);
  }

  public async Task<bool> SaveFile(string blobName, string blobData)
  {
    BlobContainerClient containerClient = _client.GetBlobContainerClient(_containerName);
    BlobClient blobClient = containerClient.GetBlobClient(blobName);

    await blobClient.UploadAsync(BinaryData.FromString(blobData), overwrite: true);

    // using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(blobData)))
    // {
    //   await blobClient.UploadAsync(ms);
    // }

    return true;
  }

  public bool SaveData(List<Entry> entries)
  {
    return true;
  }
}
