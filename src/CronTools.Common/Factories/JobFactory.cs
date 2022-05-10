using System;
using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Formatters;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace CronTools.Common.Factories;

public interface IJobFactory
{
  RunningStepContext CreateRunningStepContext(JobStepConfig jobStep, int stepNumber);
  RunningJobContext CreateRunningJobContext(JobConfig job);
  IJobArgumentResolver GetJobArgumentResolver();
}

public class JobFactory : IJobFactory
{
  private readonly List<IJobActionArgFormatter> _argFormatters;
  private readonly IJobArgumentResolver _jobArgumentResolver;

  public JobFactory(IServiceProvider serviceProvider)
  {
    _argFormatters = serviceProvider.GetServices<IJobActionArgFormatter>().ToList();
    _jobArgumentResolver = serviceProvider.GetRequiredService<IJobArgumentResolver>();
  }

  public RunningStepContext CreateRunningStepContext(JobStepConfig jobStep, int stepNumber)
  {
    // TODO: [JobFactory.CreateRunningStepContext] (TESTS) Add tests
    return new RunningStepContext(jobStep, stepNumber)
      .WithFormatters(_argFormatters);
  }

  public RunningJobContext CreateRunningJobContext(JobConfig job)
  {
    // TODO: [JobFactory.CreateRunningJobContext] (TESTS) Add tests
    return new RunningJobContext(job);
  }

  public IJobArgumentResolver GetJobArgumentResolver()
  {
    // TODO: [JobFactory.GetJobArgumentResolver] (TESTS) Add tests
    return _jobArgumentResolver;
  }
}
