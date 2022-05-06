using System;
using System.IO;
using System.Threading.Tasks;
using CronTools.Common.Extensions;
using CronTools.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

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
}
