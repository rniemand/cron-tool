using System;
using System.Linq;
using CronTools.Common.Models;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Helpers;

public interface IConditionHelper
{
  bool CanRunJobStep(RunningJobContext jobContext, RunningStepContext stepContext);
}

public class ConditionHelper : IConditionHelper
{
  private readonly ILoggerAdapter<ConditionHelper> _logger;

  public ConditionHelper(ILoggerAdapter<ConditionHelper> logger)
  {
    _logger = logger;
  }

  public bool CanRunJobStep(RunningJobContext jobContext, RunningStepContext stepContext)
  {
    // TODO: [ConditionHelper.CanRunJobStep] (TESTS) Add tests
    if (!ContainsConditions(stepContext))
      return true;

    if (!EvaluateCondition(jobContext, stepContext))
      return false;


    Console.WriteLine();
    return true;
  }

  private static bool ContainsConditions(RunningStepContext stepContext)
  {
    // TODO: [ConditionHelper.ContainsConditions] (TESTS) Add tests
    if (stepContext.JobStep.Condition is null)
      return false;

    // TODO: [ConditionHelper.ContainsConditions] (EXPAND) Add support for conditions

    return true;
  }

  private bool EvaluateCondition(RunningJobContext jobContext, RunningStepContext stepContext)
  {
    // TODO: [ConditionHelper.EvaluateCondition] (TESTS) Add tests
    var condition = stepContext.JobStep.Condition;

    if (condition is null)
      return true;

    if (condition.Expressions.Count == 0)
      return true;

    var validExpressions = condition.Expressions
      .Where(x => x.IsValid)
      .ToList();

    if (validExpressions.Count == 0)
      return true;

    foreach (var expression in validExpressions)
    {
      if (!RunExpression(jobContext, expression))
        return false;
    }

    return true;
  }

  private bool RunExpression(RunningJobContext jobContext, ConditionExpression expression)
  {
    // TODO: [ConditionHelper.RunExpression] (TESTS) Add tests
    if (!jobContext.StateValueExists(expression.Property))
    {
      _logger.LogError("Unable to resolve state value: {key}", expression.Property);
      return false;
    }

    var stateValue = jobContext.GetStateValue(expression.Property);



    Console.WriteLine();
    return true;
  }
}
