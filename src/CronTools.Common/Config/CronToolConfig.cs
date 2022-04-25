using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Config;

public class CronToolConfig
{
  public const string Key = "CronTool";

  [JsonProperty("RootDir"), JsonPropertyName("RootDir")]
  public string RootDir { get; set; }

  [JsonProperty("DirectorySeparator"), JsonPropertyName("DirectorySeparator")]
  public string DirectorySeparator { get; set; }

  [JsonProperty("JobConfigDir"), JsonPropertyName("JobConfigDir")]
  public string JobConfigDir { get; set; }

  public CronToolConfig()
  {
    // TODO: [TESTS] (CronToolConfig.CronToolConfig) Add tests
    RootDir = "./config";
    DirectorySeparator = "\\";
    JobConfigDir = "{root}jobs";
  }

  public CronToolConfig NormalizePaths(string rootDir)
  {
    // TODO: [TESTS] (CronToolConfig.NormalizePaths) Add tests
    RootDir = RootDir
      .Replace("./", rootDir.AppendIfMissing(DirectorySeparator))
      .AppendIfMissing(DirectorySeparator);

    JobConfigDir = JobConfigDir
      .Replace("{root}", RootDir)
      .AppendIfMissing(DirectorySeparator);

    return this;
  }
}