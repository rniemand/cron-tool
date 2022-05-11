using CronTools.Common.Helpers;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;

namespace CronTools.Common.Factories;

public interface IJobFactory
{
  RunningStepContext CreateRunningStepContext(JobStepConfig jobStep, int stepNumber);
  RunningJobContext CreateRunningJobContext(JobConfig job);
  IJobArgumentResolver GetJobArgumentResolver();
}

public class JobFactory : IJobFactory
{
  private readonly IJobArgumentResolver _jobArgumentResolver;
  private readonly IJobActionArgHelper _jobActionArgHelper;

  public JobFactory(
    IJobArgumentResolver jobArgumentResolver,
    IJobActionArgHelper jobActionArgHelper)
  {
    _jobArgumentResolver = jobArgumentResolver;
    _jobActionArgHelper = jobActionArgHelper;
  }

  public RunningStepContext CreateRunningStepContext(JobStepConfig jobStep, int stepNumber) =>
    // TODO: [JobFactory.CreateRunningStepContext] (TESTS) Add tests
    new(jobStep, stepNumber);

  public RunningJobContext CreateRunningJobContext(JobConfig job) =>
    // TODO: [JobFactory.CreateRunningJobContext] (TESTS) Add tests
    new(job, _jobActionArgHelper);

  public IJobArgumentResolver GetJobArgumentResolver() =>
    // TODO: [JobFactory.GetJobArgumentResolver] (TESTS) Add tests
    _jobArgumentResolver;
}
