using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace CronTools.Common.Config
{
  public class CronToolConfig
  {
    public const string Key = "CronTool";

    [JsonProperty("RootDir"), JsonPropertyName("RootDir")]
    public string RootDir { get; set; }

    public CronToolConfig()
    {
      // TODO: [TESTS] (CronToolConfig.CronToolConfig) Add tests
      RootDir = "./";
    }
  }
}
