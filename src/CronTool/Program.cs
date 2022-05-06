using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

  private static async Task Bob(string jobName, string rootDir)
  {
    ConfigureDI();

    await _serviceProvider
      .GetRequiredService<ICronRunnerService>()
      .RunAsync(new string[] {jobName});
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
      async (string name, string dir) => await Bob(name, dir),
      jobName, rootDir);

    return rootCommand;
  }
}
