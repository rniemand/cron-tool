using System;
using System.Collections.Generic;
using CronTools.Common.Models;

namespace CronTools.Common.T1.Tests.TestSupport.Builders;

public class JobStepConditionBuilder
{
  private readonly JobStepCondition _condition;
  private readonly List<string> _rawExpressions;

  public JobStepConditionBuilder()
  {
    _condition = new JobStepCondition();
    _rawExpressions = new List<string>();
  }

  public JobStepConditionBuilder WithRawExpression(string expression)
  {
    _rawExpressions.Add(expression);
    return this;
  }

  public JobStepConditionBuilder WithExpression(ConditionExpression expression)
  {
    _condition.Expressions.Add(expression);
    return this;
  }

  public JobStepConditionBuilder WithExpression(Func<ConditionExpressionBuilder, ConditionExpressionBuilder> builder)
  {
    _condition.Expressions.Add(builder.Invoke(new ConditionExpressionBuilder()).Build());
    return this;
  }

  public JobStepCondition Build()
  {
    if(_rawExpressions.Count > 0)
      _condition.RawExpressions = _rawExpressions.ToArray();

    return _condition;
  }
}
