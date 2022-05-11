using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Enums;
using CronTools.Common.Formatters;

namespace CronTools.Common.Helpers;

public interface IJobActionArgHelper
{
  Dictionary<string, string> ProcessVariables(Dictionary<string, string> variables);
  string ExecuteStringFormatters(string input);
  string ExecuteFileFormatters(string input);
  string ExecuteDirectoryFormatters(string input);
}

public class JobActionArgHelper : IJobActionArgHelper
{
  private readonly List<IJobActionArgFormatter> _formatters;

  public JobActionArgHelper(IEnumerable<IJobActionArgFormatter> formatters)
  {
    _formatters = formatters.ToList();
  }

  public Dictionary<string, string> ProcessVariables(Dictionary<string, string> variables)
  {
    // TODO: [JobActionArgHelper.ProcessVariables] (TESTS) Add tests
    var processed = new Dictionary<string, string>();

    foreach (var (key, value) in variables)
    {
      processed[key] = ProcessArgValue(value);
    }

    return processed;
  }

  public string ExecuteStringFormatters(string input)
  {
    // TODO: [JobActionArgHelper.ExecuteStringFormatters] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return input;

    var formatters = _formatters
      .Where(x => x.SupportedTypes.Any(t => t == ArgType.String))
      .ToList();

    if (formatters.Count == 0)
      return input;

    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (var formatter in formatters)
    {
      input = formatter.Format(input);
    }

    return input;
  }

  public string ExecuteFileFormatters(string input)
  {
    // TODO: [JobActionArgHelper.ExecuteFileFormatters] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return input;

    var formatters = _formatters
      .Where(x => x.SupportedTypes.Any(t => t == ArgType.File))
      .ToList();

    if (formatters.Count == 0)
      return input;

    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (var formatter in formatters)
    {
      input = formatter.Format(input);
    }

    return input;
  }

  public string ExecuteDirectoryFormatters(string input)
  {
    // TODO: [JobActionArgHelper.ExecuteDirectoryFormatters] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return input;

    var formatters = _formatters
      .Where(x => x.SupportedTypes.Any(t => t == ArgType.Directory))
      .ToList();

    if (formatters.Count == 0)
      return input;

    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (var formatter in formatters)
    {
      input = formatter.Format(input);
    }

    return input;
  }

  private string ProcessArgValue(string value)
  {
    // TODO: [JobActionArgHelper.ProcessArgValue] (TESTS) Add tests
    return _formatters.Aggregate(value, (current, formatter) => formatter.Format(current));
  }
}
