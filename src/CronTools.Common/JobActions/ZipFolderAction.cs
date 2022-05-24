using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions;

// DOCS: docs\job-actions\ZipFolder.md
public class ZipFolderAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }
  public string[] RequiredGlobals { get; }

  private readonly ILoggerAdapter<ZipFolderAction> _logger;
  private readonly IFileAbstraction _file;
  private readonly IPathAbstraction _path;
  private readonly IDirectoryAbstraction _directory;

  public ZipFolderAction(
    ILoggerAdapter<ZipFolderAction> logger,
    IFileAbstraction file,
    IPathAbstraction path,
    IDirectoryAbstraction directory)
  {
    _logger = logger;
    _file = file;
    _path = path;
    _directory = directory;

    Action = JobStepAction.ZipFolder;
    Name = JobStepAction.ZipFolder.ToString("G");
    RequiredGlobals = Array.Empty<string>();

    Args = new Dictionary<string, JobActionArg>
    {
      { "Src", JobActionArg.Directory("SourceDir", true) },
      { "Zip", JobActionArg.File("TargetZip", true) },
      { "Quick", JobActionArg.Bool("QuickZip", false) },
      { "AddBase", JobActionArg.Bool("IncludeBaseDir", false, true) },
      { "DeleteZip", JobActionArg.Bool("DeleteIfExists", false) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    var sourceDir = argResolver.ResolveDirectory(jobContext, stepContext, Args["Src"]);
    var zipFile = argResolver.ResolveFile(jobContext, stepContext, Args["Zip"]);
    var quick = argResolver.ResolveBool(jobContext, stepContext, Args["Quick"]);
    var includeBase = argResolver.ResolveBool(jobContext, stepContext, Args["AddBase"]);
    var deleteTarget = argResolver.ResolveBool(jobContext, stepContext, Args["DeleteZip"]);

    // Handle when the target zip file exists
    if (_file.Exists(zipFile))
    {
      if (!deleteTarget)
        return HandleDeleteZipDisabled(zipFile);

      DeleteExistingZipFile(zipFile);
    }

    // Ensure that the output directory exists
    var targetDirectory = _path.GetDirectoryName(zipFile);
    if(string.IsNullOrWhiteSpace(targetDirectory))
      return new JobStepOutcome(false);

    if (!EnsureDirectoryExists(targetDirectory))
      return new JobStepOutcome(false);

    // Zip the folder and return
    await Task.CompletedTask;
    var compressionLevel = quick ? CompressionLevel.Fastest : CompressionLevel.Optimal;

    ZipFile.CreateFromDirectory(sourceDir, zipFile, compressionLevel, includeBase);
    return new JobStepOutcome(true);
  }

  private bool EnsureDirectoryExists(string path)
  {
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

  private JobStepOutcome HandleDeleteZipDisabled(string zipFile)
  {
    _logger.LogWarning("ZIP file already exists: {path}", zipFile);
    return new JobStepOutcome(true);
  }

  private void DeleteExistingZipFile(string zipFile)
  {
    _logger.LogInformation("Removing existing zip file: {path}", zipFile);
    _file.Delete(zipFile);
  }
}
