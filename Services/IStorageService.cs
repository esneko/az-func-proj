using System.Collections.Generic;
using System.Threading.Tasks;
using AzFuncProj.Storage.Models;

namespace AzFuncProj.Storage.Service;

public interface IStorageService
{
  Task<bool> UploadFile(string blobName, string blobData);
  Task<bool> AddEntities(string id, List<Entry> entries);
}
