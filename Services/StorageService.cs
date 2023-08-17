using System.Collections.Generic;

namespace AzFuncProj.Storage.Service;

public class StorageService : IStorageService
{
  public bool SaveFile(string data)
  {
    return true;
  }

  public bool SaveData(List<Entry> entries)
  {
    return true;
  }
}
