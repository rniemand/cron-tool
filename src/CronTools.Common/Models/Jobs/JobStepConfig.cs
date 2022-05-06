using System.Collections.Generic;
using CronTools.Common.Enums;
using Newtonsoft.Json;

namespace CronTools.Common.Models;

public class JobStepConfig
{
  [JsonProperty("enabled")]
  public bool Enabled { get; set; } = false;

  [JsonProperty("name")]
  public string Name { get; set; } = string.Empty;

  [JsonProperty("action")]
  public JobStepAction Action { get; set; } = JobStepAction.Unknown;

  [JsonProperty("args")]
  public Dictionary<string, object> Args { get; set; } = new();
}
