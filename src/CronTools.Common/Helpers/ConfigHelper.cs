using System.Collections.Generic;
using System.Linq;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Helpers;

public static class ConfigHelper
{
  public static bool ContainsKey(Dictionary<string, object> config, string key)
  {
    return config.Count != 0 && config.Any(x => x.Key.IgnoreCaseEquals(key));
  }

  public static string GetString(Dictionary<string, object> config, string key, string fallback)
  {
    if (!ContainsKey(config, key))
      return fallback;

    var rawValue = config.First(x => x.Key.IgnoreCaseEquals(key)).Value;

    if (rawValue is string stringValue)
      return string.IsNullOrWhiteSpace(stringValue) ? fallback : stringValue;

    return CastHelper.ObjectToString(rawValue);
  }
}
