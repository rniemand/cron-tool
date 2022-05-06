using System.Collections.Generic;
using Newtonsoft.Json;

namespace CronTools.Common.Models;

public class JobConfig
{
  [JsonProperty("enabled")]
  public bool Enabled { get; set; } = false;

  [JsonProperty("name")]
  public string Name { get; set; } = string.Empty;

  [JsonProperty("steps")]
  public List<JobStepConfig> Steps { get; set; } = new();
}
