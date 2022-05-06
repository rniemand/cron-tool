using Microsoft.Extensions.Configuration;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Config;

// DOCS: docs\models\CronToolConfig.md
public class CronToolConfig
{
  public const string Key = "CronTool";

  [ConfigurationKeyName("rootDir")]
  public string RootDirectory { get; set; } = "./config";

  [ConfigurationKeyName("dirSeparator")]
  public string DirectorySeparator { get; set; } = "\\";

  [ConfigurationKeyName("jobsDirectory")]
  public string JobsDirectory { get; set; } = "{root}jobs";
  
  public CronToolConfig NormalizePaths(string rootDir)
  {
    // TODO: [TESTS] (CronToolConfig.NormalizePaths) Add tests
    RootDirectory = RootDirectory
      .Replace("./", rootDir.AppendIfMissing(DirectorySeparator))
      .AppendIfMissing(DirectorySeparator);

    JobsDirectory = JobsDirectory
      .Replace("{root}", RootDirectory)
      .AppendIfMissing(DirectorySeparator);

    return this;
  }
}
