using System;
using System.IO;
using System.Threading.Tasks;
using CronTools.Common.Formatters;
using CronTools.Common.JobActions;
using CronTools.Common.Providers;
using CronTools.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Factories;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.Metrics;

namespace CronTool;

internal class Program
{
  private static IServiceProvider _serviceProvider;

  static async Task Main(string[] args)
  {
    ConfigureDI();

    await _serviceProvider
      .GetRequiredService<ICronRunnerService>()
      .RunCrons(args);
  }

  private static void ConfigureDI()
  {
    var services = new ServiceCollection();

    var config = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", true, true)
      .Build();

    services
      // Configuration
      .AddSingleton<IConfiguration>(config)

      // Abstractions
      .AddSingleton<IDateTimeAbstraction, DateTimeAbstraction>()
      .AddSingleton<IDirectoryAbstraction, DirectoryAbstraction>()
      .AddSingleton<IFileAbstraction, FileAbstraction>()
      .AddSingleton<IEnvironmentAbstraction, EnvironmentAbstraction>()
      .AddSingleton<IPathAbstraction, PathAbstraction>()

      // Helpers
      .AddSingleton<IJsonHelper, JsonHelper>()

      // Job Actions
      .AddSingleton<IJobAction, DeleteFolderAction>()
      .AddSingleton<IJobAction, ZipFolderAction>()
      .AddSingleton<IJobAction, CopyFileAction>()
      .AddSingleton<IJobAction, DeleteFileAction>()

      // Arg Formatters
      .AddSingleton<IJobActionArgFormatter, DateTimeFormatter>()

      // Utils
      .AddSingleton<IMetricServiceUtils, MetricServiceUtils>()

      // Providers
      .AddSingleton<IConfigProvider, ConfigProvider>()

      // Factories
      .AddSingleton<IDirectoryInfoFactory, DirectoryInfoFactory>()

      // Services
      .AddSingleton<ICronRunnerService, CronRunnerService>()
      .AddSingleton<IMetricService, MetricService>()

      // Logging
      .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
      .AddLogging(loggingBuilder =>
      {
        // configure Logging with NLog
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        loggingBuilder.AddNLog(config);
      });

    _serviceProvider = services.BuildServiceProvider();
  }
}