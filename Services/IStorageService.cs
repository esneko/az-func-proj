using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
using AzFuncProj.Storage.Models;

namespace AzFuncProj.Storage.Service;

public interface IStorageService
{
  Task<bool> UploadFile(string blobName, string blobData);
  Task<bool> AddEntities(string id, List<Entry> entities);
  Task<List<T>> ListEntities<T>(string? filter) where T : class, ITableEntity;
}
