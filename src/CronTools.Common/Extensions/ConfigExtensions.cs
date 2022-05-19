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

  public static string GetString(this Dictionary<string, object> config, string key, string fallback)
  {
    // TODO: [ConfigExtensions.GetString] (TESTS) Add tests
    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;

    if (rawValue is string stringValue)
      return string.IsNullOrWhiteSpace(stringValue) ? fallback : stringValue;

    return CastHelper.ObjectToString(rawValue);
  }

  public static int GetInt(this Dictionary<string, object> config, string key, int fallback)
  {
    // TODO: [ConfigExtensions.GetInt] (TESTS) Add tests
    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;
    return CastHelper.ObjectToInt(rawValue, fallback);
  }
}
