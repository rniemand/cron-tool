using System.Collections.Generic;
using CronTools.Common.Enums;

namespace CronTools.Common.Formatters;

public interface IJobActionArgFormatter
{
  List<ArgType> SupportedTypes { get; }

  string Format(string input);
}