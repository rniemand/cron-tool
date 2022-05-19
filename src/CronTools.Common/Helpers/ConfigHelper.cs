using System.Collections.Generic;
using System.Linq;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Helpers;

public static class ConfigHelper
{
  public static bool ContainsKey(Dictionary<string, object> config, string key) =>
    // TODO: [ConfigHelper.ContainsKey] (TESTS) Add tests
    config.Count != 0 && config.Any(x => x.Key.IgnoreCaseEquals(key));

  public static string GetString(Dictionary<string, object> config, string key, string fallback)
  {
    // TODO: [ConfigHelper.GetString] (TESTS) Add tests
    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;

    if (rawValue is string stringValue)
      return string.IsNullOrWhiteSpace(stringValue) ? fallback : stringValue;

    return CastHelper.ObjectToString(rawValue);
  }

  public static int GetInt(Dictionary<string, object> config, string key, int fallback)
  {
    // TODO: [ConfigHelper.GetInt] (TESTS) Add tests
    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;
    return CastHelper.ObjectToInt(rawValue, fallback);
  }
}
