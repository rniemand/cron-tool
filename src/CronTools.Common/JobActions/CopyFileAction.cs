using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using Microsoft.Extensions.DependencyInjection;
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

  public CopyFileAction(IServiceProvider serviceProvider)
  {
    // TODO: [CopyFileAction] (TESTS) Add tests
    _logger = serviceProvider.GetRequiredService<ILoggerAdapter<CopyFileAction>>();
    _file = serviceProvider.GetRequiredService<IFileAbstraction>();
    _directory = serviceProvider.GetRequiredService<IDirectoryAbstraction>();
    _path = serviceProvider.GetRequiredService<IPathAbstraction>();
    
    Action = JobStepAction.CopyFile;
    Name = JobStepAction.CopyFile.ToString("G");

    Args = new Dictionary<string, JobActionArg>
    {
      { "Source", JobActionArg.File("Source", true) },
      { "Destination", JobActionArg.File("Target", true) },
      { "Overwrite", JobActionArg.Bool("Overwrite", false) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext)
  {
    // TODO: [TESTS] (CopyFileAction.ExecuteAsync) Add tests
    var outcome = new JobStepOutcome();

    // Handle source file missing
    var source = stepContext.ResolveFileArg(Args["Source"]);
    if (!_file.Exists(source))
      return outcome.WithError($"File {source} not found!");

    // Calculate destination folder
    var destination = stepContext.ResolveFileArg(Args["Destination"]);
    if (string.IsNullOrWhiteSpace(destination))
      return outcome.WithError("No destination provided");

    // Ensure that the destination directory exists
    var destDir = _path.GetDirectoryName(destination);
    if(string.IsNullOrWhiteSpace(destDir))
      return outcome.WithError($"Unable to calculate directory from: {destination}");

    if (!EnsureDirectoryExists(destDir))
      return outcome.WithError($"Unable to create directory: {destDir}");

    // Handle when the file exists
    var overwrite = stepContext.ResolveBoolArg(Args["Overwrite"]);
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
