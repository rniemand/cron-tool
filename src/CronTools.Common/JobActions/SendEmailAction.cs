using System.Collections.Generic;
using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;
using CronTools.Common.Resolvers;

namespace CronTools.Common.JobActions;

public class SendEmailAction : IJobAction
{
  public JobStepAction Action { get; }
  public string Name { get; }
  public Dictionary<string, JobActionArg> Args { get; }



  public Task<JobStepOutcome> ExecuteAsync(RunningJobContext jobContext, RunningStepContext stepContext, IJobArgumentResolver argResolver)
  {
    throw new System.NotImplementedException();
  }
}
