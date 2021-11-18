using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CronTools.Common.Models
{
  public class JobConfig
  {
    [JsonProperty("Enabled"), JsonPropertyName("Enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("Name"), JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonProperty("Steps"), JsonPropertyName("Steps")]
    public List<JobStepConfig> Steps { get; set; }

    public JobConfig()
    {
      // TODO: [TESTS] (JobConfig) Add tests
      Enabled = false;
      Name = string.Empty;
      Steps = new List<JobStepConfig>();
    }
  }
}
