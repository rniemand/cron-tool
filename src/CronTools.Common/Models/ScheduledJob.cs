using System;
using Newtonsoft.Json;

namespace CronTools.Common.Models;

public class ScheduledJob
{
  [JsonProperty("id")]
  public string JobId { get; set; } = string.Empty;

  [JsonProperty("nextRun")]
  public DateTimeOffset NextRun { get; set; } = DateTimeOffset.MinValue;

  [JsonProperty("lastRun")]
  public DateTimeOffset LastRun { get; set; } = DateTimeOffset.MinValue;
}
