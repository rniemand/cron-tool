using System;
using System.Linq;
using CronTools.Common.Exceptions;
using CronTools.Common.Factories;
using CronTools.Common.Formatters;
using CronTools.Common.Helpers;
using CronTools.Common.Helpers.Comparators;
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
using Rn.NetCore.MailUtils.Extensions;
using Rn.NetCore.MailUtils.Providers;
using Rn.NetCore.Metrics;
using Rn.NetCore.Metrics.Outputs;
using Rn.NetCore.Metrics.Rabbit;
using Rn.NetCore.Metrics.Rabbit.Interfaces;

namespace CronTools.Common.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddCronTool(this IServiceCollection services)
  {
    services
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

      // Metrics
      .AddSingleton<IMetricServiceUtils, MetricServiceUtils>()
      .AddSingleton<IMetricService, MetricService>()
      .AddSingleton<IRabbitFactory, RabbitFactory>()
      .AddSingleton<IRabbitConnection, RabbitConnection>()
      .AddSingleton<IMetricOutput, RabbitMetricOutput>()

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

      // Comparators
      .AddSingleton<IComparator, EqualsComparator>()
      .AddSingleton<IComparator, GreaterThanComparator>()
      .AddSingleton<IComparator, GreaterThanOrEqualComparator>()
      .AddSingleton<IComparator, LessThanComparator>()
      .AddSingleton<IComparator, LessThanOrEqualComparator>()
      .AddSingleton<IComparator, DoesNotEqualComparator>()

      // Services
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
      .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))

      // Mail vibes
      .AddRnMailUtils();

    ReplaceRnMailConfigProvider(services);

    return services;
  }

  private static void ReplaceRnMailConfigProvider(IServiceCollection services)
  {
    // Ensure that our service exists
    var descriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IRnMailConfigProvider));
    if (descriptor is null)
      throw new ServiceNotFoundException(typeof(IRnMailConfigProvider));

    // Ensure that we can remove the provider
    if (!services.Remove(descriptor))
      throw new GeneralCronToolException("Unable to remove: IRnMailConfigProvider");

    // Replace the default provider with ours
    services.AddSingleton<IRnMailConfigProvider, CronToolMailConfigProvider>();
  }
}
