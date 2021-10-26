namespace CronTools.Common.Models
{
  public class JobStepOutcome
  {
    public bool Succeeded { get; set; }

    public JobStepOutcome()
    {
      // TODO: [TESTS] (JobStepOutcome.JobStepOutcome) Add tests
      Succeeded = true;
    }

    public JobStepOutcome(bool succeeded)
      : this()
    {
      // TODO: [TESTS] (JobStepOutcome) Add tests
      Succeeded = succeeded;
    }
  }
}
