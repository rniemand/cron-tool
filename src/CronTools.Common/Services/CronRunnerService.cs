using System;
using System.Threading.Tasks;
using CronTools.Common.Factories;
using CronTools.Common.Models;
using CronTools.Common.Providers;
using CronTools.Common.Resolvers;
using CronTools.Common.Utils;
using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Services;

public interface ICronRunnerService
{
  Task RunAsync(string[] args);
}

public class CronRunnerService : ICronRunnerService
{
  private readonly ILoggerAdapter<CronRunnerService> _logger;
  private readonly IJobConfigProvider _jobConfigProvider;
  private readonly IJobActionResolver _actionResolver;
  private readonly IJobFactory _jobFactory;
  private readonly IJobUtils _jobUtils;

  public CronRunnerService(IServiceProvider serviceProvider)
  {
    // TODO: [TESTS] (CronRunnerService) Add tests
    _logger = serviceProvider.GetRequiredService<ILoggerAdapter<CronRunnerService>>();
    _jobConfigProvider = serviceProvider.GetRequiredService<IJobConfigProvider>();
    _actionResolver = serviceProvider.GetRequiredService<IJobActionResolver>();
    _jobFactory = serviceProvider.GetRequiredService<IJobFactory>();
    _jobUtils = serviceProvider.GetRequiredService<IJobUtils>();
  }

  public async Task RunAsync(string[] args)
  {
    // TODO: [TESTS] (CronRunnerService.RunAsync) Add tests
    if (args.Length == 0)
    {
      _logger.LogWarning("No jobs to run");
      return;
    }

    foreach (var job in args)
    {
      var config = _jobConfigProvider.Resolve(job);
      if (config is null) continue;

      var coreJobInfo = new CoreJobInfo(config.Name);
      var continueRunningSteps = true;
      var stepNumber = 1;

      foreach (var step in config.Steps)
      {
        if (!continueRunningSteps)
          continue;

        var resolvedAction = _actionResolver.Resolve(step);
        if (resolvedAction is null)
          throw new Exception("Unable to continue");

        var stepContext = _jobFactory.CreateRunningStepContext(coreJobInfo, step, stepNumber++);
        if (!_jobUtils.ValidateStepArgs(resolvedAction, stepContext))
          continue;

        var outcome = await resolvedAction.ExecuteAsync(stepContext);
        if (outcome.Succeeded)
          continue;

        _logger.LogError("Step failed, stopping job");
        continueRunningSteps = false;
      }
    }
  }
}
