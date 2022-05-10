using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions;

// DOCS: docs\job-actions\DeleteFile.md
public class DeleteFileAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }

  private readonly ILoggerAdapter<DeleteFileAction> _logger;
  private readonly IFileAbstraction _file;

  public DeleteFileAction(
    ILoggerAdapter<DeleteFileAction> logger,
    IFileAbstraction file)
  {
    // TODO: [TESTS] (DeleteFileAction.DeleteFileAction) Add tests
    _logger = logger;
    _file = file;

    Action = JobStepAction.DeleteFile;
    Name = JobStepAction.DeleteFile.ToString("G");

    Args = new Dictionary<string, JobActionArg>
    {
      { "Path", JobActionArg.File("Path", true) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argumentResolver)
  {
    var outcome = new JobStepOutcome();
    var path = argumentResolver.ResolveFile(jobContext, stepContext, Args["Path"]);

    if (!_file.Exists(path))
      return outcome.WithSuccess();

    if (!DeleteFile(path))
      return outcome.WithError($"Failed to delete: {path}");

    await Task.CompletedTask;
    return outcome.WithSuccess();
  }

  private bool DeleteFile(string path)
  {
    // TODO: [TESTS] (DeleteFileAction.DeleteFile) Add tests
    try
    {
      _file.Delete(path);
      return true;
    }
    catch (Exception ex)
    {
      _logger.LogUnexpectedException(ex);
      return false;
    }
  }
}
