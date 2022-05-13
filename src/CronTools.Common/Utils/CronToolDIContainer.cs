using System;
using System.IO;
using CronTools.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CronTools.Common.Utils;

public class CronToolDIContainer
{
  public static IServiceProvider ServiceProvider { get; }

  static CronToolDIContainer()
  {
    ServiceProvider = Configure();
  }

  private static IServiceProvider Configure()
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

    return services.BuildServiceProvider();
  }
}
