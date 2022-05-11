using System.Linq;
using CronTools.Common.Helpers;
using CronTools.Common.Models;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Resolvers;

public interface IJobArgumentResolver
{
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


  public string ResolveString(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveString] (TESTS) Add tests
    if (!HasArgument(stepContext, arg))
      return _actionArgHelper.ExecuteStringFormatters((string)arg.Default);

    var rawArg = stepContext.GetRawArg(arg);

    if (rawArg is string s)
      return _actionArgHelper.ExecuteStringFormatters(s);

    return _actionArgHelper.ExecuteStringFormatters((string)arg.Default);
  }

  public string ResolveDirectory(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveDirectory] (TESTS) Add tests
    if (!HasArgument(stepContext, arg))
      return _actionArgHelper.ExecuteDirectoryFormatters((string)arg.Default);

    var rawArg = stepContext.GetRawArg(arg);

    if (rawArg is string s)
      return _actionArgHelper.ExecuteDirectoryFormatters(s);

    return _actionArgHelper.ExecuteDirectoryFormatters((string)arg.Default);
  }

  public string ResolveFile(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveFile] (TESTS) Add tests
    if (!HasArgument(stepContext, arg))
      return _actionArgHelper.ExecuteFileFormatters((string)arg.Default);

    var rawArg = stepContext.GetRawArg(arg);

    if (rawArg is string s)
      return _actionArgHelper.ExecuteFileFormatters(s);

    return _actionArgHelper.ExecuteFileFormatters((string)arg.Default);
  }

  public bool ResolveBool(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveBool] (TESTS) Add tests
    if (!HasArgument(stepContext, arg))
      return (bool)arg.Default;

    var rawArg = stepContext.GetRawArg(arg);
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


  private static bool HasArgument(RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.HasArgument] (TESTS) Add tests
    // ReSharper disable once ConvertIfStatementToReturnStatement
    if (stepContext.Args.Count == 0)
      return false;

    return stepContext.Args.Any(x => x.Key.IgnoreCaseEquals(arg.Name));
  }
}
