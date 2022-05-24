using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Helpers;
using CronTools.Common.Models;
using Newtonsoft.Json.Linq;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Resolvers;

public interface IJobArgumentResolver
{
  string ResolveString(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
  string ResolveDirectory(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
  string ResolveFile(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
  List<string> ResolveFiles(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
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
    if (!HasArgument(stepContext, arg))
      return _actionArgHelper.ExecuteStringFormatters(jobContext, (string)arg.Default);

    var rawArg = stepContext.GetRawArg(arg);

    if (rawArg is string s)
      return _actionArgHelper.ExecuteStringFormatters(jobContext, s);

    return _actionArgHelper.ExecuteStringFormatters(jobContext, (string)arg.Default);
  }

  public string ResolveDirectory(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    if (!HasArgument(stepContext, arg))
      return _actionArgHelper.ExecuteDirectoryFormatters(jobContext, (string)arg.Default);

    var rawArg = stepContext.GetRawArg(arg);

    if (rawArg is string s)
      return _actionArgHelper.ExecuteDirectoryFormatters(jobContext, s);

    return _actionArgHelper.ExecuteDirectoryFormatters(jobContext, (string)arg.Default);
  }

  public string ResolveFile(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    if (!HasArgument(stepContext, arg))
      return _actionArgHelper.ExecuteFileFormatters(jobContext, (string)arg.Default);

    var rawArg = stepContext.GetRawArg(arg);

    if (rawArg is string s)
      return _actionArgHelper.ExecuteFileFormatters(jobContext, s);

    return _actionArgHelper.ExecuteFileFormatters(jobContext, (string)arg.Default);
  }

  public List<string> ResolveFiles(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    var filePaths = new List<string>();

    if (!HasArgument(stepContext, arg))
      return filePaths;

    var rawArg = stepContext.GetRawArg(arg);

    if (rawArg is string s)
    {
      filePaths.Add(_actionArgHelper.ExecuteFileFormatters(jobContext, s));
      return filePaths;
    }

    if (rawArg is not JArray jArray)
      return filePaths;

    // ReSharper disable once LoopCanBeConvertedToQuery
    foreach (var item in jArray)
    {
      if(item.Type != JTokenType.String)
        continue;

      filePaths.Add(_actionArgHelper.ExecuteFileFormatters(jobContext, item.ToString()));
    }

    return filePaths;
  }

  public bool ResolveBool(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
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
    // ReSharper disable once ConvertIfStatementToReturnStatement
    if (stepContext.Args.Count == 0)
      return false;

    return stepContext.Args.Any(x => x.Key.IgnoreCaseEquals(arg.Name));
  }
}
