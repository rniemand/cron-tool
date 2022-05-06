using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions;

// DOCS: docs\job-actions\CopyFile.md
public class CopyFileAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }

  private readonly ILoggerAdapter<CopyFileAction> _logger;
  private readonly IFileAbstraction _file;
  private readonly IDirectoryAbstraction _directory;
  private readonly IPathAbstraction _path;

  public CopyFileAction(
    IFileAbstraction file,
    IDirectoryAbstraction directory,
    IPathAbstraction path,
    ILoggerAdapter<CopyFileAction> logger)
  {
    _file = file;
    _directory = directory;
    _path = path;
    _logger = logger;
    // TODO: [TESTS] (CopyFileAction) Add tests

    Action = JobStepAction.CopyFile;
    Name = JobStepAction.CopyFile.ToString("G");

    Args = new Dictionary<string, JobActionArg>
    {
      { "Source", JobActionArg.File("Source", true) },
      { "Destination", JobActionArg.File("Target", true) },
      { "Overwrite", JobActionArg.Bool("Overwrite", false) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningStepContext context)
  {
    // TODO: [TESTS] (CopyFileAction.ExecuteAsync) Add tests
    var outcome = new JobStepOutcome();

    // Handle source file missing
    var source = context.ResolveFileArg(Args["Source"]);
    if (!_file.Exists(source))
      return outcome.WithError($"File {source} not found!");

    // Calculate destination folder
    var destination = context.ResolveFileArg(Args["Destination"]);
    if (string.IsNullOrWhiteSpace(destination))
      return outcome.WithError("No destination provided");

    // Ensure that the destination directory exists
    var destDir = _path.GetDirectoryName(destination);
    if (!EnsureDirectoryExists(destDir))
      return outcome.WithError($"Unable to create directory: {destDir}");

    // Handle when the file exists
    var overwrite = context.ResolveBoolArg(Args["Overwrite"]);
    if (_file.Exists(destination))
    {
      if (!overwrite)
        return outcome.WithSuccess($"File already exists: {destination}");

      if (!RemoveFile(destination))
        return outcome.WithError($"Unable to delete destination file: {destination}");
    }

    // Copy the file
    if (!CopyFile(source, destination))
      return outcome.WithError("Failed to copy file");

    await Task.CompletedTask;
    return outcome.WithSuccess();
  }

  private bool EnsureDirectoryExists(string path)
  {
    // TODO: [TESTS] (CopyFileAction.EnsureDirectoryExists) Add tests
    if (_directory.Exists(path))
      return true;

    try
    {
      _directory.CreateDirectory(path);
      return _directory.Exists(path);
    }
    catch (Exception ex)
    {
      _logger.LogUnexpectedException(ex);
      return false;
    }
  }

  private bool RemoveFile(string path)
  {
    // TODO: [TESTS] (CopyFileAction.RemoveFile) Add tests
    if (!_file.Exists(path))
      return true;

    try
    {
      _file.Delete(path);
      return !_file.Exists(path);
    }
    catch (Exception ex)
    {
      _logger.LogUnexpectedException(ex);
      return false;
    }
  }

  private bool CopyFile(string source, string destination)
  {
    // TODO: [TESTS] (CopyFileAction.CopyFile) Add tests
    try
    {
      _file.Copy(source, destination);
      return true;
    }
    catch (Exception ex)
    {
      _logger.LogUnexpectedException(ex);
      return false;
    }
  }
}
