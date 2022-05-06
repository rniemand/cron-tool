using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CronTools.Common.Config;
using CronTools.Common.Formatters;
using CronTools.Common.JobActions;
using CronTools.Common.Models;
using CronTools.Common.Providers;
using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Extensions;
using Rn.NetCore.Common.Factories;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Services;

public interface ICronRunnerService
{
  Task RunCrons(string[] args);
}

public class CronRunnerService : ICronRunnerService
{
  private readonly ILoggerAdapter<CronRunnerService> _logger;
  private readonly IDirectoryAbstraction _directory;
  private readonly IDirectoryInfoFactory _directoryInfoFactory;
  private readonly IFileAbstraction _file;
  private readonly IJsonHelper _jsonHelper;
  private readonly List<IJobAction> _jobActions;
  private readonly List<IJobActionArgFormatter> _argFormatters;
  private readonly CronToolConfig _config;

  private readonly Dictionary<string, string> _jobFiles;

  public CronRunnerService(
    ILoggerAdapter<CronRunnerService> logger,
    IServiceProvider serviceProvider,
    IConfigProvider configProvider,
    IDirectoryAbstraction directory,
    IDirectoryInfoFactory directoryInfoFactory,
    IFileAbstraction file,
    IJsonHelper jsonHelper)
  {
    // TODO: [TESTS] (CronRunnerService) Add tests
    _logger = logger;
    _directory = directory;
    _directoryInfoFactory = directoryInfoFactory;
    _file = file;
    _jsonHelper = jsonHelper;

    _config = configProvider.GetConfig();
    _jobActions = serviceProvider.GetServices<IJobAction>().ToList();
    _argFormatters = serviceProvider.GetServices<IJobActionArgFormatter>().ToList();
    _jobFiles = new Dictionary<string, string>();

    RefreshJobs();
  }

  public async Task RunCrons(string[] args)
  {
    // TODO: [TESTS] (CronRunnerService.RunCrons) Add tests
    if (args.Length == 0)
    {
      _logger.LogWarning("No jobs to run");
      return;
    }

    foreach (var job in args)
    {
      var config = ResolveJobConfig(job);
      if(config is null) continue;

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
      if(context.HasArgument(value.SafeName))
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

  private JobConfig ResolveJobConfig(string jobName)
  {
    // TODO: [TESTS] (CronRunnerService.ResolveJobConfig) Add tests
    if (string.IsNullOrWhiteSpace(jobName))
      return null;

    // Check to see if we have discovered the requested job
    var jobKey = jobName.LowerTrim();
    if (!_jobFiles.ContainsKey(jobKey))
    {
      _logger.LogWarning("Requested job {key} was not found in {directory}",
        jobKey,
        _config.JobsDirectory
      );

      return null;
    }

    // Ensure that the job file still exists
    var jobFilePath = _jobFiles[jobKey];
    if (!_file.Exists(jobFilePath))
    {
      _logger.LogWarning("Job file {path} no longer exists", jobFilePath);
      _jobFiles.Remove(jobKey);
      return null;
    }

    // Read contents of the config file
    var rawJson = _file.ReadAllText(jobFilePath);
    var jobConfig = _jsonHelper.DeserializeObject<JobConfig>(rawJson);

    // Handle disabled jobs
    if (!jobConfig.Enabled)
    {
      _logger.LogWarning("Requested job {name} is disabled", jobConfig.Name);
      return null;
    }

    _logger.LogInformation("Loaded config for {name} ({path})",
      jobConfig.Name,
      jobFilePath
    );

    return jobConfig;
  }

  private void EnsureDirectoriesExist()
  {
    // TODO: [TESTS] (CronRunnerService.EnsureDirectoriesExist) Add tests
    if (!_directory.Exists(_config.RootDirectory))
      _directory.CreateDirectory(_config.RootDirectory);

    if (!_directory.Exists(_config.JobsDirectory))
      _directory.CreateDirectory(_config.JobsDirectory);

    if (!_directory.Exists(_config.JobsDirectory))
      throw new DirectoryNotFoundException($"Unable to find: {_config.JobsDirectory}");
  }

  private void RefreshJobs()
  {
    // TODO: [TESTS] (CronRunnerService.RefreshJobs) Add tests
    EnsureDirectoriesExist();

    _logger.LogInformation("Refreshing jobs");
    _jobFiles.Clear();

    var directoryInfo = _directoryInfoFactory.GetDirectoryInfo(_config.JobsDirectory);
    var fileInfos = directoryInfo.GetFiles("*.json");

    if (fileInfos.Length == 0)
      return;

    _logger.LogInformation("Discovered {count} job(s) in {directory}",
      fileInfos.Length,
      _config.JobsDirectory
    );

    foreach (var fileInfo in fileInfos)
    {
      var cleanName = fileInfo.Name
        .Replace(fileInfo.Extension, "")
        .LowerTrim();

      _jobFiles[cleanName] = fileInfo.FullName;
    }
  }
}
