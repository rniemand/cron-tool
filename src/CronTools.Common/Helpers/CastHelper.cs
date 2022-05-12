using System;

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
}
