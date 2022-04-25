using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Factories;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions
{
  public class DeleteFolderAction : IJobAction
  {
    public JobStepAction Action { get; }
    public string Name { get; }
    public Dictionary<string, JobActionArg> Args { get; }

    private readonly ILoggerAdapter<DeleteFolderAction> _logger;
    private readonly IDirectoryAbstraction _directory;
    private readonly IFileAbstraction _file;
    private readonly IDirectoryInfoFactory _diFactory;

    public DeleteFolderAction(
      ILoggerAdapter<DeleteFolderAction> logger,
      IDirectoryAbstraction directoryAbstraction,
      IFileAbstraction fileAbstraction,
      IDirectoryInfoFactory diFactory)
    {
      // TODO: [TESTS] (DeleteFolderAction) Add tests
      _logger = logger;
      _directory = directoryAbstraction;
      _file = fileAbstraction;
      _diFactory = diFactory;

      Action = JobStepAction.DeleteFolder;
      Name = JobStepAction.DeleteFolder.ToString("G");

      Args = new Dictionary<string, JobActionArg>
      {
        { "Path", JobActionArg.Directory("Directory", true) },
        { "Recurse", JobActionArg.Bool("Recurse", false) }
      };
    }

    public async Task<JobStepOutcome> ExecuteAsync(RunningStepContext context)
    {
      // TODO: [TESTS] (DeleteFolderAction.ExecuteAsync) Add tests
      var path = context.ResolveDirectoryArg(Args["Path"]);

      // Nothing to do
      if (!_directory.Exists(path))
      {
        return new JobStepOutcome(true);
      }

      await Task.CompletedTask;
      var recurse = context.ResolveBoolArg(Args["Recurse"]);

      _logger.LogInformation("Deleting: {path} (recurse: {recurse})",
        path,
        recurse ? "yes" : "no"
      );

      if (recurse)
      {
        RecurseDelete(path);
        return new JobStepOutcome(true);
      }

      _logger.LogError("Need to complete me");
      return new JobStepOutcome(true);
    }

    private void RecurseDelete(string path)
    {
      // TODO: [TESTS] (DeleteFolderAction.ExecuteAsync) Add tests
      var di = _diFactory.GetDirectoryInfo(path);

      foreach (var file in di.GetFiles())
        _file.Delete(file.FullName);

      foreach (var dir in di.GetDirectories())
        RecurseDelete(dir.FullName);

      _directory.Delete(path);
    }
  }
}
