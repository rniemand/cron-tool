using CronTools.Common.Enums;

namespace CronTools.Common.Models;

public class ConditionExpression
{
  public Comparator Comparator { get; set; } = Comparator.Unknown;
  public string Property { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
}
