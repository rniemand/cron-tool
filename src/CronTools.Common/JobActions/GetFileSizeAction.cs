using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Factories;

namespace CronTools.Common.JobActions;

// DOCS: docs\job-actions\GetFileSize.md
public class GetFileSizeAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }
  public string[] RequiredGlobals { get; }
  
  private readonly IFileAbstraction _file;
  private readonly IFileInfoFactory _fileInfoFactory;

  public GetFileSizeAction(
    IFileAbstraction file,
    IFileInfoFactory fileInfoFactory)
  {
    // TODO: [GetFileSizeAction] (TESTS) Add tests
    _file = file;
    _fileInfoFactory = fileInfoFactory;

    Action = JobStepAction.GetFileSize;
    Name = JobStepAction.GetFileSize.ToString("G");
    RequiredGlobals = Array.Empty<string>();

    Args = new Dictionary<string, JobActionArg>
    {
      { "Path", JobActionArg.Files("Path", true) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    // TODO: [GetFileSizeAction.ExecuteAsync] (TESTS) Add tests
    var outcome = new JobStepOutcome();

    // Set expected defaults
    jobContext.PublishStepState(stepContext, "fileExists", false);
    jobContext.PublishStepState(stepContext, "fileSize", 0);

    var path = argResolver.ResolveFile(jobContext, stepContext, Args["Path"]);
    if (!_file.Exists(path))
      return outcome.WithFailed();

    var fileInfo = _fileInfoFactory.GetFileInfo(path);
    jobContext.PublishStepState(stepContext, "fileSize", fileInfo.Length);
    jobContext.PublishStepState(stepContext, "filePath", path);
    jobContext.PublishStepState(stepContext, "fileExists", true);

    await Task.CompletedTask;
    return outcome.WithSuccess();
  }
}
