using CronTools.Common.Factories;
using CronTools.Common.Formatters;
using CronTools.Common.Helpers;
using CronTools.Common.JobActions;
using CronTools.Common.Providers;
using CronTools.Common.Resolvers;
using CronTools.Common.Services;
using CronTools.Common.Utils;
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
      .AddSingleton<IJobStepHelper, JobStepHelper>()
      .AddSingleton<IConditionHelper, ConditionHelper>()

      // Job Actions
      .AddSingleton<IJobAction, DeleteFolderAction>()
      .AddSingleton<IJobAction, ZipFolderAction>()
      .AddSingleton<IJobAction, CopyFileAction>()
      .AddSingleton<IJobAction, DeleteFileAction>()
      .AddSingleton<IJobAction, WriteTextFileAction>()
      .AddSingleton<IJobAction, DeleteFilesAction>()
      .AddSingleton<IJobAction, GetFileSizeAction>()
      .AddSingleton<IJobAction, SendEmailAction>()

      // Arg Formatters
      .AddSingleton<IJobActionArgFormatter, DateTimeFormatter>()

      // Utils
      .AddSingleton<IMetricServiceUtils, MetricServiceUtils>()

      // Providers
      .AddSingleton<IConfigProvider, ConfigProvider>()
      .AddSingleton<IJobConfigProvider, JobConfigProvider>()
      .AddSingleton<IGlobalConfigProvider, GlobalConfigProvider>()
      .AddSingleton<IJobScheduleProvider, JobScheduleProvider>()

      // Helpers
      .AddSingleton<IJobActionArgHelper, JobActionArgHelper>()

      // Factories
      .AddSingleton<IDirectoryInfoFactory, DirectoryInfoFactory>()
      .AddSingleton<IFileInfoFactory, FileInfoFactory>()

      // Services
      .AddSingleton<IMetricService, MetricService>()
      .AddSingleton<ICronToolRunnerService, CronToolRunnerService>()
      .AddSingleton<IJobRunnerService, JobRunnerService>()
      .AddSingleton<IJobSchedulerService, JobSchedulerService>()

      // Resolvers
      .AddSingleton<IJobActionResolver, JobActionResolver>()
      .AddSingleton<IJobArgumentResolver, JobArgumentResolver>()

      // Factories
      .AddSingleton<IJobFactory, JobFactory>()

      // Utils
      .AddSingleton<IJobUtils, JobUtils>()

      // Logging
      .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
  }
}
