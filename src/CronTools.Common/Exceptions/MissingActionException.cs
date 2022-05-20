using System;
using System.Runtime.Serialization;
using CronTools.Common.Enums;

namespace CronTools.Common.Exceptions;

[Serializable]
public class MissingActionException : Exception
{
  public JobStepAction RequestedAction { get; set; }

  public MissingActionException(JobStepAction requestedAction)
    : base($"Unable to resolve requested job-action: {requestedAction:G}")
  {
    RequestedAction = requestedAction;
  }

  protected MissingActionException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  { }
}
