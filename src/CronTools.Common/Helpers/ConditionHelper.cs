using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Enums;
using CronTools.Common.Helpers.Comparators;
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
  private readonly IJobActionArgHelper _actionArgHelper;
  private readonly List<IComparator> _comparators;

  public ConditionHelper(
    ILoggerAdapter<ConditionHelper> logger,
    IJobActionArgHelper actionArgHelper,
    IEnumerable<IComparator> comparators)
  {
    _logger = logger;
    _actionArgHelper = actionArgHelper;
    _comparators = comparators.ToList();
  }

  // Public methods
  public bool CanRunJobStep(RunningJobContext jobContext, RunningStepContext stepContext)
  {
    if (!ContainsConditions(stepContext))
      return true;

    // ReSharper disable once ConvertIfStatementToReturnStatement
    if (!EvaluateStepCondition(jobContext, stepContext))
      return false;

    return true;
  }

  // Internal methods
  private static bool ContainsConditions(RunningStepContext stepContext)
  {
    if (stepContext.JobStep.Condition is null)
      return false;

    return true;
  }

  private bool EvaluateStepCondition(RunningJobContext jobContext, RunningStepContext stepContext)
  {
    var condition = stepContext.JobStep.Condition;

    return condition is null || EvaluateCondition(jobContext, condition);
  }

  private bool EvaluateCondition(RunningJobContext jobContext, JobStepCondition condition)
  {
    if (condition.Expressions.Count == 0)
      return true;

    var validExpressions = condition.Expressions
      .Where(x => x.IsValid)
      .ToList();

    if (validExpressions.Count == 0)
      return true;

    if (condition.Type == ConditionType.And)
      return EvaluateAndCondition(jobContext, validExpressions);

    if (condition.Type == ConditionType.Or)
      return EvaluateOrCondition(jobContext, validExpressions);

    _logger.LogError("Add support for {type} condition tree", condition.Type.ToString("G"));
    return false;
  }

  // Condition execution logic
  private bool EvaluateAndCondition(RunningJobContext jobContext, List<ConditionExpression> expressions)
  {
    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (var expression in expressions)
    {
      // All expressions need to be TRUE to pass
      if (!RunExpression(jobContext, expression))
        return false;
    }

    return true;
  }

  private bool EvaluateOrCondition(RunningJobContext jobContext, IEnumerable<ConditionExpression> expressions) =>
    expressions.Any(expression => RunExpression(jobContext, expression));

  private bool RunExpression(RunningJobContext jobContext, ConditionExpression expression)
  {
    if (!jobContext.StateValueExists(expression.Property))
    {
      _logger.LogError("Unable to resolve state value: {key}", expression.Property);
      return false;
    }

    if (expression.Comparator == Comparator.Unknown)
    {
      _logger.LogError("Unable to resolve comparator from expression: {ex}", expression.RawExpression);
      return false;
    }

    if (_comparators.All(x => x.Comparator != expression.Comparator))
    {
      _logger.LogError("Unable to resolve comparator for: {type}", expression.Comparator.ToString("G"));
      return false;
    }

    var sourceValue = jobContext.GetStateValue(expression.Property);
    var targetValue = _actionArgHelper.ProcessExpressionValue(jobContext, expression.Value);
    var outcome = _comparators
      .First(x => x.Comparator == expression.Comparator)
      .Compare(sourceValue, targetValue);

    _logger.LogDebug("Expression: ({expression}) evaluated to {outcome}", expression.RawExpression, outcome);
    return outcome;
  }
}
