using System;
using System.Threading.Tasks;
using CronTools.Common.Factories;
using CronTools.Common.Helpers;
using CronTools.Common.Models;
using CronTools.Common.Providers;
using CronTools.Common.Resolvers;
using CronTools.Common.Utils;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Services;

public interface IJobRunnerService
{
  Task RunJobAsync(JobConfig jobConfig);
  Task RunJobAsync(string jobFileName);
  Task RunJobsAsync(string[] jobFileNames);
}

public class JobRunnerService : IJobRunnerService
{
  private readonly ILoggerAdapter<JobRunnerService> _logger;
  private readonly IJobFactory _jobFactory;
  private readonly IJobActionResolver _actionResolver;
  private readonly IJobUtils _jobUtils;
  private readonly IConditionHelper _conditionHelper;
  private readonly IJobConfigProvider _jobConfigProvider;

  public JobRunnerService(
    ILoggerAdapter<JobRunnerService> logger,
    IJobFactory jobFactory,
    IJobActionResolver actionResolver,
    IJobUtils jobUtils,
    IConditionHelper conditionHelper,
    IJobConfigProvider jobConfigProvider)
  {
    _logger = logger;
    _jobFactory = jobFactory;
    _actionResolver = actionResolver;
    _jobUtils = jobUtils;
    _conditionHelper = conditionHelper;
    _jobConfigProvider = jobConfigProvider;
  }

  public async Task RunJobAsync(JobConfig jobConfig)
  {
    // TODO: [JobRunnerService.RunJobAsync] (TESTS) Add tests
    var continueRunningSteps = true;
    var stepNumber = 1;
    var jobContext = _jobFactory.CreateRunningJobContext(jobConfig);
    var argumentResolver = _jobFactory.GetJobArgumentResolver();
    var jobName = jobConfig.Name;

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

    _logger.LogInformation("Job '{name}' ({id}) executed", jobName, jobConfig.JobId);
  }

  public async Task RunJobAsync(string jobFileName)
  {
    // TODO: [JobRunnerService.RunJobAsync] (TESTS) Add tests
    var jobConfig = _jobConfigProvider.Resolve(jobFileName);

    if (jobConfig is null)
    {
      _logger.LogError("No job config found for file name: {name}", jobFileName);
      return;
    }

    await RunJobAsync(jobConfig);
  }

  public async Task RunJobsAsync(string[] jobFileNames)
  {
    // TODO: [JobRunnerService.RunJobsAsync] (TESTS) Add tests
    if (jobFileNames.Length == 0)
    {
      _logger.LogWarning("No jobs to run");
      return;
    }

    foreach (var jobName in jobFileNames)
    {
      await RunJobAsync(jobName);
    }
  }
}
