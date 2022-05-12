using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions;

// DOCS: docs\job-actions\GetFileSize.md
public class GetFileSizeAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }

  private readonly ILoggerAdapter<GetFileSizeAction> _logger;
  private readonly IFileAbstraction _file;

  public GetFileSizeAction(
    ILoggerAdapter<GetFileSizeAction> logger,
    IFileAbstraction file)
  {
    // TODO: [GetFileSizeAction] (TESTS) Add tests
    _logger = logger;
    _file = file;

    Action = JobStepAction.DeleteFiles;
    Name = JobStepAction.DeleteFiles.ToString("G");

    Args = new Dictionary<string, JobActionArg>
    {
      { "Path", JobActionArg.Files("Path", true) }
    };
  }

  public Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    // TODO: [GetFileSizeAction.ExecuteAsync] (TESTS) Add tests



  }
}
