using System.Collections.Generic;
using System.Threading.Tasks;
using AzFuncProj.Storage.Models;

namespace AzFuncProj.Storage.Service;

public interface IStorageService
{
  Task<bool> SaveFile(string blobName, string blobData);
  bool SaveData(List<Entry> entries);
}
