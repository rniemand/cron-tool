using System;
using CronTools.Common.Enums;

namespace CronTools.Common.Helpers;

public static class CastHelper
{
  public static string ObjectToString(object value)
  {
    // TODO: [CastHelper.ObjectToString] (TESTS) Add tests
    if (value is long longValue)
      return longValue.ToString("D");

    if (value is int intValue)
      return intValue.ToString("D");

    if (value is string stringValue)
      return stringValue;

    if (value is bool boolValue)
      return boolValue ? "true" : "false";

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

  public static bool StringToBool(string input, bool fallback = false)
  {
    // TODO: [CastHelper.StringToBool] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return fallback;

    return input.ToLower().Trim() switch
    {
      "true" => true,
      "t" => true,
      "1" => true,
      "yes" => true,
      "on" => true,
      "false" => false,
      "f" => false,
      "0" => false,
      "no" => false,
      "off" => false,
      _ => fallback
    };
  }

  public static long StringToLong(string input, long fallback = 0)
  {
    // TODO: [CastHelper.StringToLong] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return fallback;

    return long.TryParse(input, out var parsed) ? parsed : fallback;
  }

  public static int AsInt(string input, int fallback = 0)
  {
    // TODO: [CastHelper.AsInt] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return fallback;

    if (int.TryParse(input, out var parsed))
      return parsed;

    return fallback;
  }
}
