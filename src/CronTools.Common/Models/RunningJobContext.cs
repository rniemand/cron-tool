using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Extensions;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models;

public class RunningJobContext
{
  public Dictionary<string, object> Variables { get; private set; }
  public Dictionary<string, object> Globals { get; private set; }
  public Dictionary<string, object> State { get; }
  public Dictionary<string, Dictionary<string, object>> StepState { get; set; }
  public string Name { get; }

  public RunningJobContext(JobConfig job)
  {
    Name = job.Name;
    Variables = new Dictionary<string, object>();
    Globals = new Dictionary<string, object>();
    State = new Dictionary<string, object>();
    StepState = new Dictionary<string, Dictionary<string, object>>();
  }

  public RunningJobContext SetVariables(Dictionary<string, object> variables)
  {
    Variables = variables;
    return this;
  }

  public RunningJobContext SetGlobals(Dictionary<string, object> globals)
  {
    Globals = globals;
    return this;
  }

  public void PublishState(string key, object value)
  {
    State[key] = value;
  }

  public void PublishStepState(RunningStepContext stepContext, string key, object value)
  {
    var stepId = stepContext.JobStep.StepId;

    if (!StepState.ContainsKey(stepId))
      StepState[stepId] = new Dictionary<string, object>();

    StepState[stepId][key] = value;
    State[$"{stepId}.{key}"] = value;
  }

  public bool StateValueExists(string key) =>
    State.Any(x => x.Key.IgnoreCaseEquals(key));

  public object GetStateValue(string key) =>
    State.First(x => x.Key.IgnoreCaseEquals(key)).Value;

  public string GetGlobal(string key, string fallback) =>
    Globals.GetStringValue(key, fallback);
}
