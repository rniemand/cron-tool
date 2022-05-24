using System.Collections.Generic;

namespace CronTools.Common.Models;

public class JobStepOutcome
{
  public bool Succeeded { get; set; } = true;
  public string Error { get; set; } = string.Empty;
  public List<string> Messages { get; set; } = new();

  public JobStepOutcome() { }

  public JobStepOutcome(bool succeeded)
    : this()
  {
    Succeeded = succeeded;
  }

  public JobStepOutcome WithError(string error)
  {
    Succeeded = false;
    Error = error;
    return this;
  }

  public JobStepOutcome WithFailed()
  {
    Succeeded = false;
    return this;
  }

  public JobStepOutcome WithSuccess()
  {
    Succeeded = true;
    return this;
  }

  public JobStepOutcome WithSuccess(string message)
  {
    Succeeded = true;
    Messages.Add(message);
    return this;
  }
}
