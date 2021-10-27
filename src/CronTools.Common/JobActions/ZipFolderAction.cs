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
    }

    public async Task<JobStepOutcome> ExecuteAsync(RunningStepContext context)
    {
      // TODO: [TESTS] (ZipFolderAction.ExecuteAsync) Add tests

      var sourceDir = context.GetStringArg("SourceDir");
      var zipFile = context.GetStringArg("ZipFile");
      var quick = context.GetBoolArg("Quick", false);
      var includeBase = context.GetBoolArg("IncludeBase", true);
      var deleteTarget = context.GetBoolArg("DeleteTarget", true);

      // Handle when the target zip file exists
      if (_file.Exists(zipFile))
      {
        if (!deleteTarget)
        {
          _logger.Warning("ZIP file already exists: {path}", zipFile);
          return new JobStepOutcome(true);
        }

        _logger.Info("Removing existing zip file: {path}", zipFile);
        _file.Delete(zipFile);
      }

      // ZIP that shiz
      ZipFile.CreateFromDirectory(sourceDir,
        zipFile,
        quick ? CompressionLevel.Fastest : CompressionLevel.Optimal,
        includeBase
      );
      
      return new JobStepOutcome(true);
    }
  }
}
