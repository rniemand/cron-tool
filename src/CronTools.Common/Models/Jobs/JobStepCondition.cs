using System;
using CronTools.Common.Enums;
using Newtonsoft.Json;

namespace CronTools.Common.Models;

public class JobStepCondition
{
  [JsonProperty("type")]
  public ConditionType Type { get; set; } = ConditionType.And;

  [JsonProperty("expressions")]
  public string[] RawExpressions { get; set; } = Array.Empty<string>();
}
