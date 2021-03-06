using System.Linq;
using CronTools.Common.JobActions;
using CronTools.Common.Models;
using Rn.NetCore.Common.Extensions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Utils;

public interface IJobUtils
{
  bool ValidateStepArgs(IJobAction action, RunningStepContext context);
}

public class JobUtils : IJobUtils
{
  private readonly ILoggerAdapter<JobUtils> _logger;

  public JobUtils(ILoggerAdapter<JobUtils> logger)
  {
    _logger = logger;
  }

  public bool ValidateStepArgs(IJobAction action, RunningStepContext context)
  {
    if (!CheckRequiredStepArgs(action, context))
      return false;

    // ReSharper disable once ConvertIfStatementToReturnStatement
    if (action.RequiredGlobals.Length == 0)
      return true;

    return CheckRequiredGlobalArgs(action, context);
  }

  private bool CheckRequiredStepArgs(IJobAction action, RunningStepContext context)
  {
    var required = action.Args
      .Where(x => x.Value.Required)
      .ToList();

    if (required.Count == 0)
      return true;

    foreach (var (_, value) in required)
    {
      if (context.Args.Any(x => x.Key.IgnoreCaseEquals(value.Name)))
        continue;

      _logger.LogWarning(
        "Job '{name}' is missing required argument '{arg}' " +
        "(type: {argType}) for step '{stepNumber}':'{stepType}'!",
        context.JobName,
        value.Name,
        value.Type.ToString("G"),
        context.StepNumber,
        action.Name);

      return false;
    }

    return true;
  }

  private bool CheckRequiredGlobalArgs(IJobAction action, RunningStepContext context)
  {
    foreach (var globalKey in action.RequiredGlobals)
    {
      if (context.Globals.Any(x => x.Key.IgnoreCaseEquals(globalKey)))
        continue;

      _logger.LogError("Required global variable '{key}' is missing", globalKey);
      return false;
    }

    return true;
  }
}
