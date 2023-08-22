using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;

namespace AzFuncProj.Storage.Models;

public class Entry : ITableEntity
{
  public Entry() { }

  [JsonPropertyName("id")]
  public string? RowKey { get; set; }

  [JsonPropertyName("date")]
  public string? PartitionKey { get; set; }

  [JsonIgnore]
  public DateTimeOffset? Timestamp { get; set; }

  [JsonIgnore]
  public ETag ETag { get; set; }

  [JsonPropertyName("api")]
  public string API { get; set; }

  [JsonPropertyName("description")]
  public string Description { get; set; }

  [JsonPropertyName("auth")]
  public string Auth { get; set; }

  [JsonPropertyName("https")]
  public bool HTTPS { get; set; }

  [JsonPropertyName("cors")]
  public string Cors { get; set; }

  [JsonPropertyName("link")]
  public string Link { get; set; }

  [JsonPropertyName("category")]
  public string Category { get; set; }
}
