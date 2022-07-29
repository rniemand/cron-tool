using CronTools.Common.Enums;
using CronTools.Common.Models;

namespace CronTools.Common.T1.Tests.TestSupport.Builders;

public class ConditionExpressionBuilder
{
  private readonly ConditionExpression _expression;

  public ConditionExpressionBuilder()
  {
    _expression = new ConditionExpression
    {
      IsValid = true
    };
  }

  public ConditionExpressionBuilder WithComparator(Comparator comparator)
  {
    _expression.Comparator = comparator;
    return this;
  }

  public ConditionExpressionBuilder WithProperty(string property)
  {
    _expression.Property = property;
    return this;
  }

  public ConditionExpressionBuilder WithValue(string value)
  {
    _expression.Value = value;
    return this;
  }

  public ConditionExpressionBuilder WithRawExpression(string rawExpression)
  {
    _expression.RawExpression = rawExpression;
    return this;
  }

  public ConditionExpression Build() => _expression;
}
