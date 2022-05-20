using System;
using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Helpers;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Extensions;

public static class ConfigExtensions
{
  public static bool ContainsKey(this Dictionary<string, object> config, string key) =>
    // TODO: [ConfigExtensions.ContainsKey] (TESTS) Add tests
    config.Count != 0 && config.Any(x => x.Key.IgnoreCaseEquals(key));

  public static string GetStringValue(this Dictionary<string, object> config, string key, string fallback)
  {
    // TODO: [ConfigExtensions.GetStringValue] (TESTS) Add tests
    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;

    if (rawValue is string stringValue)
      return string.IsNullOrWhiteSpace(stringValue) ? fallback : stringValue;

    return CastHelper.ObjectToString(rawValue);
  }

  public static int GetIntValue(this Dictionary<string, object> config, string key, int fallback)
  {
    // TODO: [ConfigExtensions.GetIntValue] (TESTS) Add tests
    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;
    return CastHelper.ObjectToInt(rawValue, fallback);
  }

  public static bool GetBoolValue(this Dictionary<string, object> config, string key, bool fallback)
  {
    // TODO: [ConfigExtensions.GetBoolValue] (TESTS) Add tests
    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;
    return CastHelper.ObjectToBool(rawValue, fallback);
  }

  public static TEnum GetEnumValue<TEnum>(this Dictionary<string, object> config, string key, TEnum fallback)
    where TEnum : struct
  {
    // TODO: [ConfigExtensions.GetEnumValue] (TESTS) Add tests
    if (!typeof(TEnum).IsEnum)
      throw new ArgumentException($"{typeof(TEnum)} is not an enum");

    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;

    if (rawValue is TEnum enumValue)
      return enumValue;

    if (rawValue is string stringValue)
    {
      if (string.IsNullOrWhiteSpace(stringValue))
        return fallback;

      if (Enum.TryParse(typeof(TEnum), stringValue, true, out var parsed))
        return (TEnum) parsed;

      return fallback;
    }

    var source = rawValue.GetType().Name;
    var target = typeof(TEnum).Name;
    throw new InvalidCastException($"Unable to cast {source} to {target}");
  }
}
