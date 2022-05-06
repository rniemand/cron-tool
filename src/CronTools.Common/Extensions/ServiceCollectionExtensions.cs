using CronTools.Common.Formatters;
using CronTools.Common.JobActions;
using CronTools.Common.Providers;
using CronTools.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Factories;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.Metrics;

namespace CronTools.Common.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddCronTool(this IServiceCollection services)
  {
    // TODO: [ServiceCollectionExtensions.AddCronTool] (TESTS) Add tests
    return services
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
      .AddSingleton<IJobConfigProvider, JobConfigProvider>()

      // Factories
      .AddSingleton<IDirectoryInfoFactory, DirectoryInfoFactory>()

      // Services
      .AddSingleton<ICronRunnerService, CronRunnerService>()
      .AddSingleton<IMetricService, MetricService>()

      // Logging
      .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
  }
}
