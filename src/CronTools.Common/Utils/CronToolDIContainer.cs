using System;
using System.IO;
using CronTools.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CronTools.Common.Utils;

public static class CronToolDIContainer
{
  public static IServiceProvider ServiceProvider { get; }

  private static readonly string[] BasePaths = {
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    Environment.CurrentDirectory
  };

  static CronToolDIContainer()
  {
    ServiceProvider = Configure();
  }

  private static IServiceProvider Configure()
  {
    var services = new ServiceCollection();
    var configuration = "Production";

#if DEBUG
    configuration = "Development";
#endif

    var config = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("CronTool.Base.json")
      .AppendConfigLayer("CronTool.json")
      .AppendConfigLayer($"CronTool.{configuration}.json")
      .AppendConfigLayer("CronTool.Local.json")
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

  private static IConfigurationBuilder AppendConfigLayer(this IConfigurationBuilder builder, string file)
  {
    foreach (var basePath in BasePaths)
      builder.AddJsonFile(Path.Join(basePath, file), optional: true);

    return builder;
  }
}
