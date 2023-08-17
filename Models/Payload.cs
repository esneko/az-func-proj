using System.Collections.Generic;

namespace AzFuncProj.Storage.Models;

public class Payload
{
  public int count { get; set; }
  public List<Entry> entries { get; set; }
}
