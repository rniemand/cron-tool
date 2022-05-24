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
  public Dictionary<string, object> Globals { get; private set; }

  public RunningStepContext(JobStepConfig jobStep, int stepNumber)
  {
    JobName = jobStep.JobName;
    JobStep = jobStep;
    StepNumber = stepNumber;
    Globals = new Dictionary<string, object>();
  }

  public RunningStepContext SetGlobals(Dictionary<string, object> globals)
  {
    Globals = globals;
    return this;
  }

  public object GetRawArg(JobActionArg arg) =>
    Args.First(x => x.Key.IgnoreCaseEquals(arg.Name)).Value;
}
