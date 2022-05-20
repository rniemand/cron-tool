using System;
using System.Runtime.Serialization;

namespace CronTools.Common.Exceptions;

[Serializable]
public class GeneralCronToolException : Exception
{
  public GeneralCronToolException(string message)
    : base(message)
  { }

  protected GeneralCronToolException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  { }
}
