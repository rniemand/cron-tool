using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.JobActions;

// DOCS: docs\job-actions\WriteTextFileAction.md
public class WriteTextFileAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }

  private readonly ILoggerAdapter<WriteTextFileAction> _logger;
  private readonly IFileAbstraction _file;

  public WriteTextFileAction(
    ILoggerAdapter<WriteTextFileAction> logger,
    IFileAbstraction file)
  {
    // TODO: [WriteTextFileAction.WriteTextFileAction] (TESTS) Add tests
    _logger = logger;
    _file = file;

    Action = JobStepAction.WriteTextFile;
    Name = JobStepAction.WriteTextFile.ToString("G");

    Args = new Dictionary<string, JobActionArg>
    {
      { "Path", JobActionArg.File("Path", true) },
      { "Contents", JobActionArg.String("Contents", true) },
      { "Overwrite", JobActionArg.Bool("Overwrite", false) }
    };
  }

  public async Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext)
  {
    // TODO: [WriteTextFileAction.ExecuteAsync] (TESTS) Add tests
    var outcome = new JobStepOutcome();

    var contents = stepContext.ResolveStringArg(Args["Contents"]);
    

    Console.WriteLine();
    return outcome;
  }
}
