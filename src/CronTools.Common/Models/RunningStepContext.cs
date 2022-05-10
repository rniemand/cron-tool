using System.Collections.Generic;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models;

public class RunningStepContext
{
  public string JobName { get; set; }
  public JobStepConfig JobStep { get; set; }
  public Dictionary<string, object> NormalizedArgs { get; }
  public int StepNumber { get; set; }

  public RunningStepContext(JobStepConfig jobStep, int stepNumber)
  {
    // TODO: [TESTS] (RunningStepContext.RunningStepContext) Add tests
    JobName = jobStep.JobName;
    JobStep = jobStep;
    StepNumber = stepNumber;

    NormalizedArgs = new Dictionary<string, object>();
    foreach (var (key, value) in jobStep.Args)
    {
      NormalizedArgs[key.LowerTrim()] = value;
    }
  }
}
