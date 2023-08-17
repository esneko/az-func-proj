using System.Collections.Generic;

namespace AzFuncProj.Storage.Service;

public class StorageService : IStorageService
{
  public bool SaveFile(string payload)
  {
    log.LogInformation($"C# Timer trigger function saved the payload: {payload}");
    return true;
  }

  // public bool SaveData(List<Entry> entries)
  // {
  //   log.LogInformation($"C# Timer trigger function saved the payload: {entries.count()}");
  //   return true;
  // }
}
