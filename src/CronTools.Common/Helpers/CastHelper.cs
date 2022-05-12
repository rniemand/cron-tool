using System;
using CronTools.Common.Enums;

namespace CronTools.Common.Helpers;

public static class CastHelper
{
  public static string ObjectToString(object value)
  {
    // TODO: [CastHelper.ObjectToString] (TESTS) Add tests
    if (value is long longValue)
    {
      return longValue.ToString("D");
    }

    if (value is string stringValue)
    {
      return stringValue;
    }

    throw new Exception("Not supported");
  }

  public static Comparator ToComparator(string value)
  {
    // TODO: [CastHelper.ToComparator] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(value))
      return Comparator.Unknown;
    
    return value.ToLower().Trim() switch
    {
      "=" => Comparator.Equals,
      "<" => Comparator.LessThan,
      "<=" => Comparator.LessThanOrEqual,
      ">" => Comparator.GreaterThan,
      ">=" => Comparator.GreaterThanOrEqual,
      "!=" => Comparator.DoesNotEqual,
      _ => Comparator.Unknown
    };
  }
}
