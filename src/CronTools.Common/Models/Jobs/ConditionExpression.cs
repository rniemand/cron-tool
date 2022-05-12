using System.Diagnostics;
using CronTools.Common.Enums;

namespace CronTools.Common.Models;

[DebuggerDisplay("{Property} {Comparator} {Value}")]
public class ConditionExpression
{
  public Comparator Comparator { get; set; } = Comparator.Unknown;
  public string Property { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
  public bool IsValid { get; set; } = false;
  public string RawExpression { get; set; } = string.Empty;
}
