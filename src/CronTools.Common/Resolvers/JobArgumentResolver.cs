using System.Linq;
using CronTools.Common.Enums;
using CronTools.Common.Helpers;
using CronTools.Common.Models;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Resolvers;

public interface IJobArgumentResolver
{
  bool HasArgument(RunningStepContext stepContext, string argName);
  string ResolveString(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
  string ResolveDirectory(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
  string ResolveFile(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
  bool ResolveBool(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
}

public class JobArgumentResolver : IJobArgumentResolver
{
  private readonly IJobActionArgHelper _actionArgHelper;

  public JobArgumentResolver(IJobActionArgHelper actionArgHelper)
  {
    _actionArgHelper = actionArgHelper;
  }

  public bool HasArgument(RunningStepContext stepContext, string argName)
  {
    // TODO: [JobArgumentResolver.HasArgument] (TESTS) Add tests
    // ReSharper disable once ConvertIfStatementToReturnStatement
    if (stepContext.Args.Count == 0)
      return false;

    return stepContext.Args.Any(x => x.Key.IgnoreCaseEquals(argName));
  }

  public string ResolveString(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveString] (TESTS) Add tests
    if (!HasArgument(stepContext, arg.Name))
      return _actionArgHelper.ExecuteStringFormatters((string)arg.Default, ArgType.String);

    var rawArg = stepContext.Args.First(x => x.Key.IgnoreCaseEquals(arg.Name)).Value;

    if (rawArg is string s)
      return _actionArgHelper.ExecuteStringFormatters(s, ArgType.String);

    return _actionArgHelper.ExecuteStringFormatters((string)arg.Default, ArgType.String);
  }

  public string ResolveDirectory(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveDirectory] (TESTS) Add tests
    if (!HasArgument(stepContext, arg.Name))
      return (string)arg.Default;

    var rawArg = stepContext.Args.First(x => x.Key.IgnoreCaseEquals(arg.Name)).Value;

    if (rawArg is string s)
      return s;

    return (string)arg.Default;
  }

  public string ResolveFile(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveFile] (TESTS) Add tests
    if (!HasArgument(stepContext, arg.Name))
      return _actionArgHelper.ExecuteStringFormatters((string)arg.Default, ArgType.File);

    var rawArg = stepContext.Args.First(x => x.Key.IgnoreCaseEquals(arg.Name)).Value;

    if (rawArg is string s)
      return _actionArgHelper.ExecuteStringFormatters(s, ArgType.File);

    return _actionArgHelper.ExecuteStringFormatters((string)arg.Default, ArgType.File);
  }

  public bool ResolveBool(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveBool] (TESTS) Add tests
    if (!HasArgument(stepContext, arg.Name))
      return (bool)arg.Default;

    var rawArg = stepContext.Args.First(x => x.Key.IgnoreCaseEquals(arg.Name)).Value;
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
}
