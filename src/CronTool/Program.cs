using System;
using System.IO;
using System.Threading.Tasks;
using CronTools.Common.Extensions;
using CronTools.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.CommandLine;

namespace CronTool;

internal class Program
{
  private static IServiceProvider _serviceProvider;

  static async Task Main(string[] args)
  {
    await BuildCommand().InvokeAsync(args);
  }

  private static void ConfigureDI()
  {
    var services = new ServiceCollection();

    var config = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", true, true)
      .Build();

    services
      .AddSingleton<IConfiguration>(config)
      .AddCronTool()
      .AddLogging(loggingBuilder =>
      {
        // configure Logging with NLog
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        loggingBuilder.AddNLog(config);
      });

    _serviceProvider = services.BuildServiceProvider();
  }

  private static async Task RunTool(string jobName, string rootDir)
  {
    GenerateAppSettings(rootDir);

    ConfigureDI();

    await _serviceProvider
      .GetRequiredService<ICronRunnerService>()
      .RunAsync(new[] {jobName});
  }

  private static RootCommand BuildCommand()
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

    var rootCommand = new RootCommand {rootDir, jobName};
    rootCommand.Description = "Rn.CronTool";

    rootCommand.SetHandler(
      async (string name, string dir) => await RunTool(name, dir),
      jobName, rootDir);

    return rootCommand;
  }

  private static void GenerateAppSettings(string rootDir)
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
}
