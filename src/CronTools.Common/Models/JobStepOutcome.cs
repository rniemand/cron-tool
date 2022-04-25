using System.Collections.Generic;

namespace CronTools.Common.Models;

public class JobStepOutcome
{
  public bool Succeeded { get; set; }
  public string Error { get; set; }
  public List<string> Messages { get; set; }

  public JobStepOutcome()
  {
    // TODO: [TESTS] (JobStepOutcome.JobStepOutcome) Add tests
    Succeeded = true;
    Error = string.Empty;
    Messages = new List<string>();
  }

  public JobStepOutcome(bool succeeded)
    : this()
  {
    // TODO: [TESTS] (JobStepOutcome) Add tests
    Succeeded = succeeded;
  }

  public JobStepOutcome WithError(string error)
  {
    // TODO: [TESTS] (JobStepOutcome.WithError) Add tests
    Succeeded = false;
    Error = error;
    return this;
  }

  public JobStepOutcome WithSuccess()
  {
    // TODO: [TESTS] (JobStepOutcome.WithSuccess) Add tests
    Succeeded = true;
    return this;
  }

  public JobStepOutcome WithSuccess(string message)
  {
    // TODO: [TESTS] (JobStepOutcome.WithSuccess) Add tests
    Succeeded = true;
    Messages.Add(message);
    return this;
  }
}