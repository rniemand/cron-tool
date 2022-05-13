using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Helpers;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models;

public class RunningJobContext
{
  public Dictionary<string, object> Variables { get; }
  public Dictionary<string, object> State { get; }
  public Dictionary<string, Dictionary<string, object>> StepState { get; set; }
  public string Name { get; }

  public RunningJobContext(JobConfig job, IJobActionArgHelper argHelper)
  {
    // TODO: [RunningJobContext] (TESTS) Add tests
    Name = job.Name;
    Variables = argHelper.ProcessVariables(job.Variables);
    State = new Dictionary<string, object>();
    StepState = new Dictionary<string, Dictionary<string, object>>();
  }

  public void PublishState(string key, object value)
  {
    // TODO: [RunningJobContext.PublishState] (TESTS) Add tests
    State[key] = value;
  }

  public void PublishStepState(RunningStepContext stepContext, string key, object value)
  {
    // TODO: [RunningJobContext.PublishStepState] (TESTS) Add tests
    var stepId = stepContext.JobStep.StepId;

    if (!StepState.ContainsKey(stepId))
      StepState[stepId] = new Dictionary<string, object>();

    StepState[stepId][key] = value;
    State[$"{stepId}.{key}"] = value;
  }

  public bool StateValueExists(string key)
  {
    // TODO: [RunningJobContext.StateValueExists] (TESTS) Add tests
    return State.Any(x => x.Key.IgnoreCaseEquals(key));
  }

  public object GetStateValue(string key)
  {
    // TODO: [RunningJobContext.GetStateValue] (TESTS) Add tests
    return State.First(x => x.Key.IgnoreCaseEquals(key)).Value;
  }
}
