using CronTools.Common.Helpers;
using CronTools.Common.Models;
using CronTools.Common.Providers;
using CronTools.Common.Resolvers;

namespace CronTools.Common.Factories;

public interface IJobFactory
{
  RunningStepContext CreateRunningStepContext(RunningJobContext jobContext, JobStepConfig jobStep, int stepNumber);
  RunningJobContext CreateRunningJobContext(JobConfig job);
  IJobArgumentResolver GetJobArgumentResolver();
}

public class JobFactory : IJobFactory
{
  private readonly IJobArgumentResolver _jobArgumentResolver;
  private readonly IJobActionArgHelper _jobActionArgHelper;
  private readonly IGlobalConfigProvider _globalConfigProvider;

  public JobFactory(
    IJobArgumentResolver jobArgumentResolver,
    IJobActionArgHelper jobActionArgHelper,
    IGlobalConfigProvider globalConfigProvider)
  {
    _jobArgumentResolver = jobArgumentResolver;
    _jobActionArgHelper = jobActionArgHelper;
    _globalConfigProvider = globalConfigProvider;
  }

  public RunningStepContext CreateRunningStepContext(RunningJobContext jobContext, JobStepConfig jobStep, int stepNumber) =>
    new RunningStepContext(jobStep, stepNumber)
      .SetGlobals(jobContext.Globals);

  public RunningJobContext CreateRunningJobContext(JobConfig job) =>
    new RunningJobContext(job)
      .SetVariables(_jobActionArgHelper.ProcessVariables(job.Variables))
      .SetGlobals(_globalConfigProvider.GetGlobalConfig());

  public IJobArgumentResolver GetJobArgumentResolver() =>
    _jobArgumentResolver;
}
