using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CronTools.Common.Enums;
using CronTools.Common.Formatters;
using CronTools.Common.Models;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Helpers;

public interface IJobActionArgHelper
{
  Dictionary<string, object> ProcessVariables(Dictionary<string, object> variables);
  string ExecuteStringFormatters(RunningJobContext jobContext, string input);
  string ExecuteFileFormatters(RunningJobContext jobContext, string input);
  string ExecuteDirectoryFormatters(RunningJobContext jobContext, string input);
  string ProcessExpressionValue(RunningJobContext jobContext, string value);
}

public class JobActionArgHelper : IJobActionArgHelper
{
  private readonly List<IJobActionArgFormatter> _formatters;

  public JobActionArgHelper(IEnumerable<IJobActionArgFormatter> formatters)
  {
    _formatters = formatters.ToList();
  }


  public Dictionary<string, object> ProcessVariables(Dictionary<string, object> variables)
  {
    // TODO: [JobActionArgHelper.ProcessVariables] (TESTS) Add tests
    var processed = new Dictionary<string, object>();

    foreach (var (key, value) in variables)
    {
      if (value is string stringValue)
      {
        processed[key] = ProcessArgValue(stringValue);
      }
      else
      {
        processed[key] = value;
      }
    }

    return processed;
  }

  public string ExecuteStringFormatters(RunningJobContext jobContext, string input)
  {
    // TODO: [JobActionArgHelper.ExecuteStringFormatters] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return input;

    input = ProcessPlaceholders(jobContext, input);
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

  public string ExecuteFileFormatters(RunningJobContext jobContext, string input)
  {
    // TODO: [JobActionArgHelper.ExecuteFileFormatters] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return input;

    input = ProcessPlaceholders(jobContext, input);
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

  public string ExecuteDirectoryFormatters(RunningJobContext jobContext, string input)
  {
    // TODO: [JobActionArgHelper.ExecuteDirectoryFormatters] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return input;

    input = ProcessPlaceholders(jobContext, input);
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

  public string ProcessExpressionValue(RunningJobContext jobContext, string value)
  {
    // TODO: [JobActionArgHelper.ProcessExpressionValue] (TESTS) Add tests
    return ProcessPlaceholders(jobContext, value);
  }


  private string ProcessArgValue(string value)
  {
    // TODO: [JobActionArgHelper.ProcessArgValue] (TESTS) Add tests
    return _formatters.Aggregate(value, (current, formatter) => formatter.Format(current));
  }

  private static string ProcessPlaceholders(RunningJobContext jobContext, string input)
  {
    // TODO: [JobActionArgHelper.ProcessPlaceholders] (TESTS) Add tests
    input = HandleVariables(jobContext, input);
    input = HandleState(jobContext, input);

    return input;
  }

  private static string HandleVariables(RunningJobContext jobContext, string input)
  {
    // TODO: [JobActionArgHelper.HandleVariables] (TESTS) Add tests

    // (\${var:([^}]+)})
    const string rxPattern = "(\\${var:([^}]+)})";
    if (!input.MatchesRegex(rxPattern))
      return input;

    foreach (Match match in input.GetRegexMatches(rxPattern))
    {
      var varKey = match.Groups[2].Value;
      if(!jobContext.Variables.Any(x => x.Key.IgnoreCaseEquals(varKey)))
        continue;

      var resolved = jobContext.Variables
        .First(x => x.Key.IgnoreCaseEquals(varKey))
        .Value;
      
      input = input.Replace(match.Groups[1].Value, CastHelper.ObjectToString(resolved));
    }

    return input;
  }

  private static string HandleState(RunningJobContext jobContext, string input)
  {
    // TODO: [JobActionArgHelper.HandleState] (TESTS) Add tests

    // (\${var:([^}]+)})
    const string rxPattern = "(\\${state:([^}]+)})";
    if (!input.MatchesRegex(rxPattern))
      return input;

    foreach (Match match in input.GetRegexMatches(rxPattern))
    {
      var varKey = match.Groups[2].Value;
      if (!jobContext.State.Any(x => x.Key.IgnoreCaseEquals(varKey)))
        continue;

      var resolved = jobContext.State
        .First(x => x.Key.IgnoreCaseEquals(varKey))
        .Value;
      
      input = input.Replace(match.Groups[1].Value, CastHelper.ObjectToString(resolved));
    }

    return input;
  }
}
