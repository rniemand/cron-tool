using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Enums;
using CronTools.Common.Formatters.Interfaces;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models;

public class RunningStepContext
{
  public CoreJobInfo JobInfo { get; set; }
  public JobStepConfig Config { get; set; }
  public Dictionary<string, object> NormalizedArgs { get; }
  public int StepNumber { get; set; }
  public List<IJobActionArgFormatter> Formatters { get; set; }

  public RunningStepContext(CoreJobInfo job, JobStepConfig config, int stepNumber)
  {
    // TODO: [TESTS] (RunningStepContext.RunningStepContext) Add tests
    JobInfo = job;
    Config = config;
    StepNumber = stepNumber;
    Formatters = new List<IJobActionArgFormatter>();

    NormalizedArgs = new Dictionary<string, object>();

    if (config?.Args == null)
      return;

    foreach (var (key, value) in config.Args)
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
    
  public bool HasArgument(JobActionArg arg)
  {
    // TODO: [TESTS] (RunningStepContext.HasArgument) Add tests
    return HasArgument(arg.SafeName);
  }

  public string ResolveDirectoryArg(JobActionArg arg)
  {
    // TODO: [TESTS] (RunningStepContext.ResolveDirectoryArg) Add tests
    if (!HasArgument(arg.SafeName))
      return (string)arg.Default;

    var rawArg = NormalizedArgs[arg.SafeName];

    if (rawArg is string s)
      return s;

    return (string)arg.Default;
  }

  public string ResolveFileArg(JobActionArg arg)
  {
    // TODO: [TESTS] (RunningStepContext.ResolveFileArg) Add tests
    if (!HasArgument(arg.SafeName))
      return ExecuteStringFormatters((string) arg.Default, ArgType.File);
      
    var rawArg = NormalizedArgs[arg.SafeName];

    if (rawArg is string s)
      return ExecuteStringFormatters(s, ArgType.File);

    return ExecuteStringFormatters((string) arg.Default, ArgType.File);
  }

  public bool ResolveBoolArg(JobActionArg arg)
  {
    // TODO: [TESTS] (RunningStepContext.ResolveBoolArg) Add tests
    if (!HasArgument(arg))
      return (bool)arg.Default;

    var rawArg = NormalizedArgs[arg.SafeName];
    if (rawArg is bool b)
      return b;

    if (rawArg is string strRawAre)
    {
      if (bool.TryParse(strRawAre, out var parsed))
      {
        return parsed;
      }

      return (bool)arg.Default;
    }

    if (rawArg is int)
    {
      return (int)rawArg == 1;
    }

    return (bool)arg.Default;
  }

  private string ExecuteStringFormatters(string input, ArgType argType)
  {
    // TODO: [TESTS] (RunningStepContext.ExecuteStringFormatters) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return input;

    var formatters = Formatters
      .Where(x => x.SupportedTypes.Any(t => t == argType))
      .ToList();

    if (formatters.Count == 0)
      return input;

    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (var formatter in formatters)
    {
      input = formatter.Format(input);
    }

    return input;
  }
}
