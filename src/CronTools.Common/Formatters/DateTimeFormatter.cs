using System.Collections.Generic;
using CronTools.Common.Enums;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Formatters;

// DOCS: docs\string-formatters\DateTimeFormatter.md
public class DateTimeFormatter : IJobActionArgFormatter
{
  public List<ArgType> SupportedTypes { get; }
  private readonly IDateTimeAbstraction _dateTime;

  public DateTimeFormatter(IDateTimeAbstraction dateTime)
  {
    // TODO: [TESTS] (DateTimeFormatter) Add tests
    _dateTime = dateTime;

    SupportedTypes = new List<ArgType>
    {
      ArgType.File
    };
  }

  public string Format(string input)
  {
    // TODO: [TESTS] (DateTimeFormatter.Format) Add tests
    // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
    const string regex = @"{date:([^}]+)}";
    if (!input.MatchesRegex(regex))
      return input;

    var match = input.GetRegexMatch(regex);
    var now = _dateTime.Now;

    return input.Replace(
      match.Groups[0].Value,
      now.ToString(match.Groups[1].Value));
  }
}
