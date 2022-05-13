using System;
using System.Threading.Tasks;
using CronTools.Common.Factories;
using CronTools.Common.Helpers;
using CronTools.Common.Providers;
using CronTools.Common.Resolvers;
using CronTools.Common.Utils;
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
  private readonly IConditionHelper _conditionHelper;

  public CronRunnerService(
    ILoggerAdapter<CronRunnerService> logger,
    IJobConfigProvider jobConfigProvider,
    IJobActionResolver actionResolver,
    IJobFactory jobFactory,
    IJobUtils jobUtils,
    IConditionHelper conditionHelper)
  {
    _logger = logger;
    _jobConfigProvider = jobConfigProvider;
    _actionResolver = actionResolver;
    _jobFactory = jobFactory;
    _jobUtils = jobUtils;
    _conditionHelper = conditionHelper;
  }

  public async Task RunAsync(string[] args)
  {
    // TODO: [TESTS] (CronRunnerService.RunAsync) Add tests
    if (args.Length == 0)
    {
      _logger.LogWarning("No jobs to run");
      return;
    }

    foreach (var jobName in args)
    {
      var jobConfig = _jobConfigProvider.Resolve(jobName);
      if (jobConfig is null)
        continue;

      var continueRunningSteps = true;
      var stepNumber = 1;
      var jobContext = _jobFactory.CreateRunningJobContext(jobConfig);
      var argumentResolver = _jobFactory.GetJobArgumentResolver();

      foreach (var step in jobConfig.Steps)
      {
        if (!continueRunningSteps)
          continue;

        var resolvedAction = _actionResolver.Resolve(step);
        if (resolvedAction is null)
          throw new Exception("Unable to continue");

        // Ensure that the current job step arguments are valid
        var stepContext = _jobFactory.CreateRunningStepContext(jobContext, step, stepNumber++);
        var stepArgsValid = _jobUtils.ValidateStepArgs(resolvedAction, stepContext);
        if (!stepArgsValid)
        {
          if (!step.QuitOnFailure) continue;
          _logger.LogError("Job '{job}' step '{step}' has invalid arguments, stopping", jobName, step.StepId);
          break;
        }

        // Handle step conditions, quit if we need to
        var canRunJobStep = _conditionHelper.CanRunJobStep(jobContext, stepContext);
        var stopOnFailure = step.Condition?.StopOnFailure ?? false;
        if (!canRunJobStep && stopOnFailure)
        {
          _logger.LogInformation("Job '{job}' step '{step}' conditions failed, stopping job.", jobName, step.StepId);
          break;
        }

        // Handle a successful step
        var outcome = await resolvedAction.ExecuteAsync(jobContext, stepContext, argumentResolver);
        if (outcome.Succeeded)
          continue;

        if (!step.QuitOnFailure)
          continue;

        // Step failed, can't continue on error, log and stop
        _logger.LogError("Step failed, stopping job");
        continueRunningSteps = false;
      }
    }
  }
}
