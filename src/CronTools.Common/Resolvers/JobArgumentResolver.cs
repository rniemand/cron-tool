using System.Collections.Generic;
using System.Linq;
using CronTools.Common.Enums;
using CronTools.Common.Formatters;
using CronTools.Common.Models;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Resolvers;

public interface IJobArgumentResolver
{
  bool HasArgument(RunningStepContext stepContext, string argName);
  string ResolveString(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
  string ResolveDirectory(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
  string ResolveFile(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg);
}

public class JobArgumentResolver : IJobArgumentResolver
{
  private readonly List<IJobActionArgFormatter> _formatters;

  public JobArgumentResolver(IEnumerable<IJobActionArgFormatter> formatters)
  {
    _formatters = formatters.ToList();
  }

  public bool HasArgument(RunningStepContext stepContext, string argName)
  {
    // TODO: [JobArgumentResolver.HasArgument] (TESTS) Add tests
    return stepContext.NormalizedArgs.Count != 0 && stepContext.NormalizedArgs.ContainsKey(argName.LowerTrim());
  }

  public string ResolveString(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveString] (TESTS) Add tests
    if (!HasArgument(stepContext, arg.SafeName))
      return ExecuteStringFormatters((string)arg.Default, ArgType.String);

    var rawArg = stepContext.NormalizedArgs[arg.SafeName];

    if (rawArg is string s)
      return ExecuteStringFormatters(s, ArgType.String);

    return ExecuteStringFormatters((string)arg.Default, ArgType.String);
  }

  public string ResolveDirectory(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveDirectory] (TESTS) Add tests
    if (!HasArgument(stepContext, arg.SafeName))
      return (string)arg.Default;

    var rawArg = stepContext.NormalizedArgs[arg.SafeName];

    if (rawArg is string s)
      return s;

    return (string)arg.Default;
  }

  public string ResolveFile(RunningJobContext jobContext, RunningStepContext stepContext, JobActionArg arg)
  {
    // TODO: [JobArgumentResolver.ResolveFile] (TESTS) Add tests
    if (!HasArgument(stepContext, arg.SafeName))
      return ExecuteStringFormatters((string)arg.Default, ArgType.File);

    var rawArg = stepContext.NormalizedArgs[arg.SafeName];

    if (rawArg is string s)
      return ExecuteStringFormatters(s, ArgType.File);

    return ExecuteStringFormatters((string)arg.Default, ArgType.File);
  }

  private string ExecuteStringFormatters(string input, ArgType argType)
  {
    // TODO: [JobArgumentResolver.ExecuteStringFormatters] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(input))
      return input;

    var formatters = _formatters
      .Where(x => x.SupportedTypes.Any(t => t == argType))
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
}
