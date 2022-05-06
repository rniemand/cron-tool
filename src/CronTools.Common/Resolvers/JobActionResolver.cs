using System;
using System.Collections.Generic;
using System.Linq;
using CronTools.Common.JobActions;
using CronTools.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Resolvers;

public interface IJobActionResolver
{
  IJobAction? Resolve(JobStepConfig jobStep);
}

public class JobActionResolver : IJobActionResolver
{
  private readonly ILoggerAdapter<JobActionResolver> _logger;
  private readonly List<IJobAction> _jobActions;

  public JobActionResolver(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetRequiredService<ILoggerAdapter<JobActionResolver>>();
    _jobActions = serviceProvider.GetServices<IJobAction>().ToList();
  }

  public IJobAction? Resolve(JobStepConfig jobStep)
  {
    // TODO: [JobActionResolver.Resolve] (TESTS) Add tests
    var resolved = _jobActions.FirstOrDefault(x => x.Action == jobStep.Action);

    if (resolved is not null)
      return resolved;

    _logger.LogWarning("Unable to resolve action '{action}' for '{job}' step: {step}",
      jobStep.Action.ToString("G"),
      jobStep.JobName,
      jobStep.Name);

    return null;
  }
}
