using System;
using System.IO;
using System.Threading.Tasks;
using CronTools.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.Common.Metrics;
using Rn.NetCore.Common.Metrics.Interfaces;

namespace CronTool
{
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

        // Utils
        .AddSingleton<IMetricServiceUtils, MetricServiceUtils>()

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
}
