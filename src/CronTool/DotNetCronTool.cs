using System;
using System.CommandLine;
using System.IO;
using System.Threading.Tasks;
using CronTools.Common.Services;
using CronTools.Common.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace CronTool;

public static class DotNetCronTool
{
  public static RootCommand BuildCommand()
  {
    var rootDir = new Option<string>(
      "--rootDir",
      getDefaultValue: () => Environment.CurrentDirectory,
      description: "Root directory to use when resolving jobs");
    rootDir.AddAlias("-r");

    var jobName = new Option<string>(
      "--job",
      getDefaultValue: () => string.Empty,
      description: "The name of the job you wish to run");
    jobName.AddAlias("-j");

    var rootCommand = new RootCommand { rootDir, jobName };
    rootCommand.Description = "Rn.CronTool";

    rootCommand.SetHandler(
      async (string name, string dir) => await RunTool(name, dir),
      jobName, rootDir);

    return rootCommand;
  }

  public static void GenerateAppSettings(string rootDir)
  {
    var template = @"{
        ""Logging"": {
          ""LogLevel"": {
            ""Default"": ""Debug"",
            ""Microsoft"": ""Warning"",
            ""Microsoft.Hosting.Lifetime"": ""Information""
          }
        },
        ""CronTool"": {
          ""rootDir"": ""{rootDir}"",
          ""dirSeparator"": ""\\"",
          ""jobsDirectory"": ""{root}jobs""
        } 
      }"
      .Replace("{rootDir}", rootDir.Replace("\\", "\\\\"));

    var targetFile = Path.Join(Environment.CurrentDirectory, "appsettings.json");
    if (File.Exists(targetFile)) File.Delete(targetFile);
    File.WriteAllText(targetFile, template);
  }

  public static async Task RunTool(string jobName, string rootDir)
  {
    GenerateAppSettings(rootDir);
    
    await CronToolDIContainer.ServiceProvider
      .GetRequiredService<ICronRunnerService>()
      .RunJobsAsync(new[] { jobName });
  }
}

