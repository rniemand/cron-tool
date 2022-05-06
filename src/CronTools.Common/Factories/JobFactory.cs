using System;
using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Formatters;
using CronTools.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CronTools.Common.Factories;

public interface IJobFactory
{
  RunningStepContext CreateRunningStepContext(JobStepConfig jobStep, int stepNumber);
}

public class JobFactory : IJobFactory
{
  private readonly List<IJobActionArgFormatter> _argFormatters;

  public JobFactory(IServiceProvider serviceProvider)
  {
    _argFormatters = serviceProvider.GetServices<IJobActionArgFormatter>().ToList();
  }

  public RunningStepContext CreateRunningStepContext(JobStepConfig jobStep, int stepNumber)
  {
    // TODO: [JobFactory.CreateRunningStepContext] (TESTS) Add tests
    return new RunningStepContext(jobStep, stepNumber)
      .WithFormatters(_argFormatters);
  }
}
