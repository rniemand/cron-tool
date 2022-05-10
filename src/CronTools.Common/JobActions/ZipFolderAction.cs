using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions;

// DOCS: docs\job-actions\ZipFolder.md
public class ZipFolderAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }

  private readonly ILoggerAdapter<ZipFolderAction> _logger;
  private readonly IFileAbstraction _file;
  private readonly IPathAbstraction _path;
  private readonly IDirectoryAbstraction _directory;

  public ZipFolderAction(IServiceProvider serviceProvider)
  {
    // TODO: [TESTS] (ZipFolderAction) Add tests
    _logger = serviceProvider.GetRequiredService<ILoggerAdapter<ZipFolderAction>>();
    _file = serviceProvider.GetRequiredService<IFileAbstraction>();
    _path = serviceProvider.GetRequiredService<IPathAbstraction>();
    _directory = serviceProvider.GetRequiredService<IDirectoryAbstraction>();

    Action = JobStepAction.ZipFolder;
    Name = JobStepAction.ZipFolder.ToString("G");

    Args = new Dictionary<string, JobActionArg>
    {
      { "Src", JobActionArg.Directory("SourceDir", true) },
      { "Zip", JobActionArg.File("TargetZip", true) },
      { "Quick", JobActionArg.Bool("QuickZip", false) },
      { "AddBase", JobActionArg.Bool("IncludeBaseDir", false, true) },
      { "DeleteZip", JobActionArg.Bool("DeleteIfExists", false) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext)
  {
    // TODO: [TESTS] (ZipFolderAction.ExecuteAsync) Add tests
    var sourceDir = stepContext.ResolveDirectoryArg(Args["Src"]);
    var zipFile = stepContext.ResolveFileArg(Args["Zip"]);
    var quick = stepContext.ResolveBoolArg(Args["Quick"]);
    var includeBase = stepContext.ResolveBoolArg(Args["AddBase"]);
    var deleteTarget = stepContext.ResolveBoolArg(Args["DeleteZip"]);

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
    // TODO: [TESTS] (ZipFolderAction.EnsureDirectoryExists) Add tests
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
    // TODO: [TESTS] (ZipFolderAction.HandleDeleteZipDisabled) Add tests
    _logger.LogWarning("ZIP file already exists: {path}", zipFile);
    return new JobStepOutcome(true);
  }

  private void DeleteExistingZipFile(string zipFile)
  {
    // TODO: [TESTS] (ZipFolderAction.DeleteExistingZipFile) Add tests
    _logger.LogInformation("Removing existing zip file: {path}", zipFile);
    _file.Delete(zipFile);
  }
}
