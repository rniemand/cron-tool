using System.Collections.Generic;

namespace CronTools.Common.Models;

public class RunningJobContext
{
  public Dictionary<string, string> Variables { get; }
  public string Name { get; }

  public RunningJobContext(JobConfig job)
  {
    // TODO: [RunningJobContext] (TESTS) Add tests
    Variables = job.Variables;
    Name = job.Name;
  }
}
