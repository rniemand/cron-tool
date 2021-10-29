using System.Collections.Generic;
using System.Linq;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models
{
  public class RunningStepContext
  {
    public CoreJobInfo JobInfo { get; set; }
    public JobStepConfig Config { get; set; }
    public Dictionary<string, object> NormalizedArgs { get; }
    public int StepNumber { get; set; }

    public RunningStepContext(CoreJobInfo job, JobStepConfig config, int stepNumber)
    {
      // TODO: [TESTS] (RunningStepContext.RunningStepContext) Add tests
      JobInfo = job;
      Config = config;
      StepNumber = stepNumber;

      NormalizedArgs = new Dictionary<string, object>();

      if (config?.Args == null)
        return;

      foreach (var (key, value) in config?.Args)
      {
        NormalizedArgs[key.LowerTrim()] = value;
      }
    }

    public bool HasArgument(string key)
    {
      // TODO: [TESTS] (RunningStepContext.HasArgument) Add tests
      return NormalizedArgs.Count != 0 && NormalizedArgs.ContainsKey(key.LowerTrim());
    }

    public bool GetBoolArg(string key, bool fallback)
    {
      // TODO: [TESTS] (RunningStepContext.GetBoolArg) Add tests
      if (!HasArgument(key))
        return fallback;

      var rawArg = NormalizedArgs[key.LowerTrim()];
      if (rawArg is bool)
        return (bool)rawArg;

      if (rawArg is string)
      {
        if (bool.TryParse((string)rawArg, out var parsed))
        {
          return parsed;
        }

        return fallback;
      }

      if (rawArg is int)
      {
        return (int)rawArg == 1;
      }

      return fallback;
    }

    public string GetStringArg(string key)
    {
      // TODO: [TESTS] (RunningStepContext.GetStringArg) Add tests
      if (!HasArgument(key))
        return string.Empty;

      var rawArg = NormalizedArgs[key.LowerTrim()];

      if (rawArg is string)
      {
        return (string)rawArg;
      }

      return rawArg.ToString();
    }

    public bool HasRequiredArgs(List<string> args)
    {
      // TODO: [TESTS] (RunningStepContext.HasRequiredArgs) Add tests
      if (args.Count == 0)
      {
        return true;
      }

      return args.All(HasArgument);
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
        return string.Empty;

      var rawArg = NormalizedArgs[arg.SafeName];

      if (rawArg is string s)
        return s;

      return rawArg.ToString();
    }

    public bool ResolveBoolArg(JobActionArg arg)
    {
      // TODO: [TESTS] (RunningStepContext.ResolveBoolArg) Add tests
      if (!HasArgument(arg))
        return (bool)arg.Default;

      var rawArg = NormalizedArgs[arg.SafeName];
      if (rawArg is bool b)
        return b;

      if (rawArg is string)
      {
        if (bool.TryParse((string)rawArg, out var parsed))
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
  }
}
