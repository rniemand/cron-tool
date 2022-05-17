using CronTools.Common.Enums;
using Newtonsoft.Json;

namespace CronTools.Common.Models;

// DOCS: docs\configuration\JobSchedule.md
public class JobSchedule
{
  [JsonProperty("frequency")]
  public ScheduleFrequency Frequency { get; set; } = ScheduleFrequency.Hour;

  [JsonProperty("intValue")]
  public int IntValue { get; set; } = 12;

  [JsonProperty("runOnStart")]
  public bool RunOnStart { get; set; } = false;

  [JsonProperty("timeOfDay")]
  public string TimeOfDay { get; set; } = string.Empty;
}
