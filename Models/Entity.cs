using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;

namespace AzFuncProj.Storage.Models;

public class Entity : Entry, ITableEntity
{
  public Entity() { }

  [JsonPropertyName("id")]
  public string? RowKey { get; set; }

  [JsonPropertyName("date")]
  public string? PartitionKey { get; set; }

  [JsonIgnore]
  public DateTimeOffset? Timestamp { get; set; }

  [JsonIgnore]
  public ETag ETag { get; set; }
}
