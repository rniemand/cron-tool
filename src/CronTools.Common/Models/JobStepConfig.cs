using System.Collections.Generic;
using System.Text.Json.Serialization;
using CronTools.Common.Enums;
using Newtonsoft.Json;

namespace CronTools.Common.Models
{
  public class JobStepConfig
  {
    [JsonProperty("Enabled"), JsonPropertyName("Enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("Name"), JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonProperty("Action"), JsonPropertyName("Action")]
    public JobStepAction Action { get; set; }

    [JsonProperty("Args"), JsonPropertyName("Args")]
    public Dictionary<string, object> Args { get; set; }

    public JobStepConfig()
    {
      // TODO: [TESTS] (JobStepConfig) Add tests
      Enabled = false;
      Name = string.Empty;
      Action = JobStepAction.Unknown;
      Args = new Dictionary<string, object>();
    }
  }
}
