using System.Collections.Generic;
using System.Linq;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models;

public class RunningJobContext
{
  public Dictionary<string, object> Variables { get; private set; }
  public Dictionary<string, string> Globals { get; private set; }
  public Dictionary<string, object> State { get; }
  public Dictionary<string, Dictionary<string, object>> StepState { get; set; }
  public string Name { get; }

  public RunningJobContext(JobConfig job)
  {
    // TODO: [RunningJobContext] (TESTS) Add tests
    Name = job.Name;
    Variables = new Dictionary<string, object>();
    Globals = new Dictionary<string, string>();
    State = new Dictionary<string, object>();
    StepState = new Dictionary<string, Dictionary<string, object>>();
  }

  public RunningJobContext SetVariables(Dictionary<string, object> variables)
  {
    // TODO: [RunningJobContext.SetVariables] (TESTS) Add tests
    Variables = variables;
    return this;
  }

  public RunningJobContext SetGlobals(Dictionary<string, string> globals)
  {
    // TODO: [RunningJobContext.SetGlobals] (TESTS) Add tests
    Globals = globals;
    return this;
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

  public string GetGlobal(string key, string fallback)
  {
    // TODO: [RunningJobContext.GetGlobal] (TESTS) Add tests
    if (!Globals.Any(x => x.Key.IgnoreCaseEquals(key)))
      return fallback;

    return Globals.First(x => x.Key.IgnoreCaseEquals(key)).Value;
  }
}
