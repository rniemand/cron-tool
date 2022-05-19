using System;
using System.Runtime.Serialization;

namespace CronTools.Common.Exceptions;

[Serializable]
public class ServiceNotFoundException : Exception
{
  public Type? ServiceType { get; set; }

  public ServiceNotFoundException(Type serviceType)
    : base($"Unable to find service of type: {serviceType.Name}")
  {
    ServiceType = serviceType;
  }

  protected ServiceNotFoundException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  { }
}
