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

    public RunningStepContext(CoreJobInfo job, JobStepConfig config)
    {
      // TODO: [TESTS] (RunningStepContext.RunningStepContext) Add tests
      JobInfo = job;
      Config = config;

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
        return (bool) rawArg;

      if (rawArg is string)
      {
        if (bool.TryParse((string) rawArg, out var parsed))
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
  }
}
