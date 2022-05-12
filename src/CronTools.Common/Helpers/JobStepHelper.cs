using System;
using CronTools.Common.Models;

namespace CronTools.Common.Helpers;

public interface IJobStepHelper
{
  void InitializeSteps(JobConfig jobConfig);
}

public class JobStepHelper : IJobStepHelper
{
  public void InitializeSteps(JobConfig jobConfig)
  {
    // TODO: [JobStepHelper.InitializeSteps] (TESTS) Add tests
    var currentStepNumber = 1;

    foreach (var jobStep in jobConfig.Steps)
    {
      jobStep.JobName = jobConfig.Name;
      SetStepId(jobStep, currentStepNumber++);
    }


    Console.WriteLine();
  }

  private static void SetStepId(JobStepConfig jobStep, int stepNumber)
  {
    // TODO: [JobStepHelper.SetStepId] (TESTS) Add tests
    jobStep.StepNumber = stepNumber;

    if (!string.IsNullOrWhiteSpace(jobStep.StepId))
      return;

    jobStep.StepId = "step_" + jobStep.StepNumber.ToString("D").PadLeft(2, '0');
  }
}
