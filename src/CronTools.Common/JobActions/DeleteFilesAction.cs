using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Rn.NetCore.Common.Abstractions;

namespace CronTools.Common.JobActions;

public class DeleteFilesAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }
  public string[] RequiredGlobals { get; }
  
  private readonly IFileAbstraction _file;

  public DeleteFilesAction(IFileAbstraction file)
  {
    _file = file;

    Action = JobStepAction.DeleteFiles;
    Name = JobStepAction.DeleteFiles.ToString("G");
    RequiredGlobals = Array.Empty<string>();

    Args = new Dictionary<string, JobActionArg>
    {
      { "Paths", JobActionArg.Files("Paths", true) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    var outcome = new JobStepOutcome();

    var paths = argResolver.ResolveFiles(jobContext, stepContext, Args["Paths"]);
    if (paths.Count == 0)
      return outcome.WithSuccess();

    // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
    foreach (var filePath in paths)
    {
      if(!_file.Exists(filePath))
        continue;

      _file.Delete(filePath);
    }

    // Great success
    await Task.CompletedTask;
    return outcome.WithSuccess();
  }
}
