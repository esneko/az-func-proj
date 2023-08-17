using System.Collections.Generic;

namespace AzFuncProj.Storage.Service;

public interface IStorageService
{
  bool SaveFile(string payload);
  // bool SaveData(List<Entry> entries);
}
