using CronTools.Common.Enums;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Helpers.Comparators;

public class EqualsComparator : IComparator
{
  public Comparator Comparator => Comparator.Equals;

  private readonly ILoggerAdapter<EqualsComparator> _logger;

  public EqualsComparator(ILoggerAdapter<EqualsComparator> logger)
  {
    _logger = logger;
  }

  public bool Compare(object source, string target)
  {
    _logger.LogDebug("Comparing: '{source}' = '{target}'", source, target);

    if (source is bool boolValue)
      return CastHelper.StringToBool(target) == boolValue;

    _logger.LogError("Add support for {type}", source.GetType().Name);
    return false;
  }
}
