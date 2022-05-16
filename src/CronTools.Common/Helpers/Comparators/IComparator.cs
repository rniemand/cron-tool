using CronTools.Common.Enums;

namespace CronTools.Common.Helpers.Comparators;

public interface IComparator
{
  Comparator Comparator { get; }

  bool Compare(object source, string target);
}
