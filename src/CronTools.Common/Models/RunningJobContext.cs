using System.Collections.Generic;
using CronTools.Common.Helpers;

namespace CronTools.Common.Models;

public class RunningJobContext
{
  public Dictionary<string, string> Variables { get; }
  public Dictionary<string, string> State { get; }
  public string Name { get; }

  public RunningJobContext(JobConfig job, IJobActionArgHelper argHelper)
  {
    // TODO: [RunningJobContext] (TESTS) Add tests
    Name = job.Name;
    Variables = argHelper.ProcessVariables(job.Variables);
    State = new Dictionary<string, string>();
  }

  public void PublishState(string key, string value)
  {
    // TODO: [RunningJobContext.PublishState] (TESTS) Add tests
    State[key] = value;
  }
}
