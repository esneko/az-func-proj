using System.Text.Json.Serialization;

namespace AzFuncProj.Storage.Models;

public class Entry
{
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

  [JsonIgnore]
  public DateTime? Date { get; init; }
}
