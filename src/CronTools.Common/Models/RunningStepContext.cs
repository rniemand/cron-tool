namespace CronTools.Common.Models
{
  public class RunningStepContext
  {
    public CoreJobInfo JobInfo { get; set; }
    public JobStepConfig Config { get; set; }

    public RunningStepContext(CoreJobInfo job, JobStepConfig config)
    {
      // TODO: [TESTS] (RunningStepContext.RunningStepContext) Add tests
      JobInfo = job;
      Config = config;
    }
  }
}
