using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;

namespace CronTools.Common.JobActions;

public interface IJobAction
{
  JobStepAction Action { get; }
  string Name { get; }
  Dictionary<string, JobActionArg> Args { get; }
  string[] RequiredGlobals { get; }

  Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver);
}
