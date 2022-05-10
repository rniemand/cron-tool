using System.Collections.Generic;
using CronTools.Common.Formatters;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models;

public class RunningStepContext
{
  public string JobName { get; set; }
  public JobStepConfig JobStep { get; set; }
  public Dictionary<string, object> NormalizedArgs { get; }
  public int StepNumber { get; set; }
  public List<IJobActionArgFormatter> Formatters { get; set; }

  public RunningStepContext(JobStepConfig jobStep, int stepNumber)
  {
    // TODO: [TESTS] (RunningStepContext.RunningStepContext) Add tests
    JobName = jobStep.JobName;
    JobStep = jobStep;
    StepNumber = stepNumber;
    Formatters = new List<IJobActionArgFormatter>();

    NormalizedArgs = new Dictionary<string, object>();
    foreach (var (key, value) in jobStep.Args)
    {
      NormalizedArgs[key.LowerTrim()] = value;
    }
  }

  public RunningStepContext WithFormatters(List<IJobActionArgFormatter> formatters)
  {
    // TODO: [TESTS] (RunningStepContext.WithFormatters) Add tests
    Formatters = formatters;
    return this;
  }

  public bool HasArgument(string key)
  {
    // TODO: [TESTS] (RunningStepContext.HasArgument) Add tests
    return NormalizedArgs.Count != 0 && NormalizedArgs.ContainsKey(key.LowerTrim());
  }
}
