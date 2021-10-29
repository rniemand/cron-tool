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

    public ZipFolderAction(
      IFileAbstraction file,
      ILoggerAdapter<ZipFolderAction> logger)
    {
      // TODO: [TESTS] (ZipFolderAction) Add tests
      _file = file;
      _logger = logger;

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

      // ZIP that shit
      ZipFile.CreateFromDirectory(sourceDir,
        zipFile,
        quick ? CompressionLevel.Fastest : CompressionLevel.Optimal,
        includeBase
      );
      
      return new JobStepOutcome(true);
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
