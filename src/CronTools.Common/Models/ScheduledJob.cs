using System;
using Newtonsoft.Json;

namespace CronTools.Common.Models;

public class ScheduledJob
{
  [JsonProperty("id")]
  public string JobId { get; set; } = string.Empty;

  [JsonProperty("nextRun")]
  public DateTime NextRun { get; set; } = DateTime.MinValue;

  [JsonProperty("lastRun")]
  public DateTime LastRun { get; set; } = DateTime.MinValue;

  [JsonProperty("name")]
  public string JobName { get; set; } = string.Empty;
}
