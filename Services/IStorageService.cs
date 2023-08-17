using System.Collections.Generic;
using AzFuncProj.Storage.Models;

namespace AzFuncProj.Storage.Service;

public interface IStorageService
{
  bool SaveFile(string data);
  bool SaveData(List<Entry> entries);
}
