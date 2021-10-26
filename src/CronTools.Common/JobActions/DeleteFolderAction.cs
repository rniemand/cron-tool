using System.Threading.Tasks;
using CronTools.Common.Enums;
using CronTools.Common.Models;

namespace CronTools.Common.JobActions
{
  public class DeleteFolderAction : IJobAction
  {
    public JobStepAction Action { get; }
    public string Name { get; }
    
    public DeleteFolderAction()
    {
      // TODO: [TESTS] (DeleteFolderAction) Add tests
      Action = JobStepAction.DeleteFolder;
      Name = JobStepAction.DeleteFolder.ToString("G");
    }

    public async Task<JobStepOutcome> ExecuteAsync(RunningStepContext context)
    {
      // TODO: [TESTS] (DeleteFolderAction.ExecuteAsync) Add tests


      return new JobStepOutcome(true);
    }
  }
}
