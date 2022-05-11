using System.Collections.Generic;
using System.Linq;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models;

public class RunningStepContext
{
  public string JobName { get; set; }
  public JobStepConfig JobStep { get; set; }
  public int StepNumber { get; set; }
  public Dictionary<string, object> Args => JobStep.Args;

  public RunningStepContext(JobStepConfig jobStep, int stepNumber)
  {
    // TODO: [TESTS] (RunningStepContext.RunningStepContext) Add tests
    JobName = jobStep.JobName;
    JobStep = jobStep;
    StepNumber = stepNumber;
  }

  public object GetRawArg(JobActionArg arg)
  {
    // TODO: [RunningStepContext.GetRawArg] (TESTS) Add tests
    return Args.First(x => x.Key.IgnoreCaseEquals(arg.Name)).Value;
  }
}
