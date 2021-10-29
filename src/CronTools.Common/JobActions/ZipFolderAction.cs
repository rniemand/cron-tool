using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions
{
  public class ZipFolderAction : IJobAction
  {
    public JobStepAction Action { get; }
    public string Name { get; }
    public Dictionary<string, JobActionArg> Args { get; }

    private readonly ILoggerAdapter<ZipFolderAction> _logger;
    private readonly IFileAbstraction _file;
    private readonly IPathAbstraction _path;
    private readonly IDirectoryAbstraction _directory;

    public ZipFolderAction(
      IFileAbstraction file,
      ILoggerAdapter<ZipFolderAction> logger,
      IPathAbstraction path,
      IDirectoryAbstraction directory)
    {
      // TODO: [TESTS] (ZipFolderAction) Add tests
      _file = file;
      _logger = logger;
      _path = path;
      _directory = directory;

      Action = JobStepAction.ZipFolder;
      Name = JobStepAction.ZipFolder.ToString("G");

      Args = new Dictionary<string, JobActionArg>
      {
        { "Src", JobActionArg.Directory("SourceDir", true) },
        { "Zip", JobActionArg.File("TargetZip", true) },
        { "Quick", JobActionArg.Bool("Quick", false) },
        { "AddBase", JobActionArg.Bool("IncludeBaseDirectory", false, true) },
        { "DeleteZip", JobActionArg.Bool("DeleteTargetZipIfExists", false) }
      };
    }

    public async Task<JobStepOutcome> ExecuteAsync(RunningStepContext context)
    {
      // TODO: [TESTS] (ZipFolderAction.ExecuteAsync) Add tests
      var sourceDir = context.ResolveDirectoryArg(Args["Src"]);
      var zipFile = context.ResolveFileArg(Args["Zip"]);
      var quick = context.ResolveBoolArg(Args["Quick"]);
      var includeBase = context.ResolveBoolArg(Args["AddBase"]);
      var deleteTarget = context.ResolveBoolArg(Args["DeleteZip"]);

      // Handle when the target zip file exists
      if (_file.Exists(zipFile))
      {
        if (!deleteTarget)
          return HandleDeleteZipDisabled(zipFile);

        DeleteExistingZipFile(zipFile);
      }

      // Ensure that the output directory exists
      var targetDirectory = _path.GetDirectoryName(zipFile);
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
      _logger.Warning("ZIP file already exists: {path}", zipFile);
      return new JobStepOutcome(true);
    }

    private void DeleteExistingZipFile(string zipFile)
    {
      // TODO: [TESTS] (ZipFolderAction.DeleteExistingZipFile) Add tests
      _logger.Info("Removing existing zip file: {path}", zipFile);
      _file.Delete(zipFile);
    }
  }
}
