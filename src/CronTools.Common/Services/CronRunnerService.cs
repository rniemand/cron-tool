using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CronTools.Common.Formatters;
using CronTools.Common.JobActions;
using CronTools.Common.Models;
using CronTools.Common.Providers;
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
  private readonly List<IJobAction> _jobActions;
  private readonly List<IJobActionArgFormatter> _argFormatters;
  private readonly IJobConfigProvider _jobConfigProvider;

  public CronRunnerService(
    ILoggerAdapter<CronRunnerService> logger,
    IServiceProvider serviceProvider,
    IJobConfigProvider jobConfigProvider)
  {
    // TODO: [TESTS] (CronRunnerService) Add tests
    _logger = logger;
    _jobConfigProvider = jobConfigProvider;

    _jobActions = serviceProvider.GetServices<IJobAction>().ToList();
    _argFormatters = serviceProvider.GetServices<IJobActionArgFormatter>().ToList();
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

      var coreJobInfo = GenerateCoreJobInfo(config);
      var continueRunningSteps = true;
      var stepNumber = 1;

      foreach (var step in config.Steps)
      {
        if (!continueRunningSteps)
          continue;

        var resolvedAction = _jobActions
          .FirstOrDefault(x => x.Action == step.Action);

        if (resolvedAction is null)
        {
          _logger.LogError("Unable to resolve action {name} for job {job}",
            step.Action.ToString("G"),
            config.Name
          );

          throw new Exception("Unable to continue");
        }

        var stepContext = new RunningStepContext(coreJobInfo, step, stepNumber++)
          .WithFormatters(_argFormatters);

        if (!ValidateStepArgs(resolvedAction, stepContext))
          continue;

        var outcome = await resolvedAction.ExecuteAsync(stepContext);
        if (outcome.Succeeded)
          continue;

        _logger.LogError("Step failed, stopping job");
        continueRunningSteps = false;
      }
    }
  }

  private bool ValidateStepArgs(IJobAction action, RunningStepContext context)
  {
    // TODO: [TESTS] (CronRunnerService.ValidateStepArgs) Add tests
    if (!CheckRequiredStepArgs(action, context))
      return false;

    return true;
  }

  private bool CheckRequiredStepArgs(IJobAction action, RunningStepContext context)
  {
    // TODO: [TESTS] (CronRunnerService.CheckRequiredStepArgs) Add tests
    var required = action.Args
      .Where(x => x.Value.Required)
      .ToList();

    if (required.Count == 0)
      return true;

    foreach (var (_, value) in required)
    {
      if (context.HasArgument(value.SafeName))
        continue;

      _logger.LogWarning(
        "Job '{name}' is missing required argument '{arg}' " +
        "(type: {argType}) for step '{stepNumber}':'{stepType}'!",
        context.JobInfo.Name,
        value.Name,
        value.Type.ToString("G"),
        context.StepNumber,
        action.Name
      );

      return false;
    }

    return true;
  }

  private static CoreJobInfo GenerateCoreJobInfo(JobConfig jobConfig)
  {
    // TODO: [TESTS] (CronRunnerService.GenerateCoreJobInfo) Add tests
    return new CoreJobInfo
    {
      Name = jobConfig.Name
    };
  }
}
