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

  public static int StringToInt(string input, int fallback = 0)
  {
    // TODO: [CastHelper.StringToInt] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return fallback;

    return int.TryParse(input, out var parsed) ? parsed : fallback;
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

  public static int ObjectToInt(object value, int fallback = 0)
  {
    // TODO: [CastHelper.ObjectToInt] (TESTS) Add tests
    if (value is int intValue)
      return intValue;

    if (value is string stringValue)
    {
      if (string.IsNullOrWhiteSpace(stringValue))
        return fallback;

      if(int.TryParse(stringValue, out var parsedInt))
        return parsedInt;

      return fallback;
    }

    if (value is long longValue)
    {
      if (longValue < int.MaxValue)
        return (int) longValue;

      throw new InvalidCastException($"Value {longValue} is bigger than int.MaxValue");
    }

    var source = value.GetType().Name;
    var target = nameof(Int32);
    throw new InvalidCastException($"Unable to cast {source} to {target}");
  }
}
