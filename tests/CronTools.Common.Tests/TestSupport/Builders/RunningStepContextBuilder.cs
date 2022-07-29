using System;
using CronTools.Common.Models;

namespace CronTools.Common.T1.Tests.TestSupport.Builders;

public class RunningStepContextBuilder
{
  private readonly JobStepConfig _jobStepConfig;
  private readonly int _stepNumber;

  public RunningStepContextBuilder()
  {
    _jobStepConfig = new JobStepConfig();
    _stepNumber = 1;
  }

  public RunningStepContextBuilder WithCondition(Func<JobStepConditionBuilder, JobStepConditionBuilder> builder)
  {
    _jobStepConfig.Condition = builder.Invoke(new JobStepConditionBuilder()).Build();
    return this;
  }

  public RunningStepContext Build()
  {
    return new RunningStepContext(_jobStepConfig, _stepNumber);
  }
}
