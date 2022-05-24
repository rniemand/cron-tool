using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions;

// DOCS: docs\job-actions\WriteTextFileAction.md
public class WriteTextFileAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }
  public string[] RequiredGlobals { get; }

  private readonly ILoggerAdapter<WriteTextFileAction> _logger;
  private readonly IFileAbstraction _file;

  public WriteTextFileAction(
    ILoggerAdapter<WriteTextFileAction> logger,
    IFileAbstraction file)
  {
    _logger = logger;
    _file = file;

    Action = JobStepAction.WriteTextFile;
    Name = JobStepAction.WriteTextFile.ToString("G");
    RequiredGlobals = Array.Empty<string>();

    Args = new Dictionary<string, JobActionArg>
    {
      { "Path", JobActionArg.File("Path", true) },
      { "Contents", JobActionArg.String("Contents", true) },
      { "Overwrite", JobActionArg.Bool("Overwrite", false) },
      { "PublishAs", JobActionArg.String("PublishAs", false) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    var outcome = new JobStepOutcome();
    var filePath = argResolver.ResolveFile(jobContext, stepContext, Args["Path"]);
    var replace = argResolver.ResolveBool(jobContext, stepContext, Args["Overwrite"]);

    // If the file exists and we are not allowed to overwrite it, return
    if (_file.Exists(filePath) && !replace)
    {
      _logger.LogWarning("File '{path}' already exists.", filePath);
      return outcome.WithError($"File '{filePath}' already exists");
    }

    // Handle existing file - we are allowed to remove at this point
    if (!_file.Exists(filePath))
    {
      _logger.LogInformation("Replacing file: {path}", filePath);
      _file.Delete(filePath);
    }

    // Write contents to the file and return
    var contents = argResolver.ResolveString(jobContext, stepContext, Args["Contents"]);
    var publishAs = argResolver.ResolveString(jobContext, stepContext, Args["PublishAs"]);
    await _file.WriteAllTextAsync(filePath, contents);

    if (!string.IsNullOrWhiteSpace(publishAs))
      jobContext.PublishState(publishAs, filePath);

    return outcome.WithSuccess($"Contents written to: {filePath}");
  }
}
