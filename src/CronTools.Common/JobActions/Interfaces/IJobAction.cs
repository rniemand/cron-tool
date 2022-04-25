using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;

namespace CronTools.Common.JobActions;

public interface IJobAction
{
  JobStepAction Action { get; }
  string Name { get; }
  Dictionary<string, JobActionArg> Args { get; }

  Task<JobStepOutcome> ExecuteAsync(RunningStepContext context);
}