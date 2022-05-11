using System.Collections.Generic;
using CronTools.Common.Helpers;

namespace CronTools.Common.Models;

public class RunningJobContext
{
  public Dictionary<string, string> Variables { get; }
  public string Name { get; }

  public RunningJobContext(JobConfig job, IJobActionArgHelper argHelper)
  {
    // TODO: [RunningJobContext] (TESTS) Add tests
    Name = job.Name;
    Variables = argHelper.ProcessVariables(job.Variables);
  }
}
