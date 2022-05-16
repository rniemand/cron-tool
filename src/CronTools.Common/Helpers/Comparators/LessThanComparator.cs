using CronTools.Common.Enums;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Helpers.Comparators;

public class LessThanComparator : IComparator
{
  public Comparator Comparator => Comparator.LessThan;

  private readonly ILoggerAdapter<LessThanComparator> _logger;

  public LessThanComparator(ILoggerAdapter<LessThanComparator> logger)
  {
    _logger = logger;
  }

  public bool Compare(object source, string target)
  {
    _logger.LogDebug("Comparing: '{source}' < '{target}'", source, target);

    if (source is long longValue)
      return longValue < CastHelper.StringToLong(target);

    _logger.LogError("Add support for {type}", source.GetType().Name);
    return false;
  }
}
