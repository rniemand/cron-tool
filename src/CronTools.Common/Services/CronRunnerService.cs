using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CronTools.Common.Config;
using CronTools.Common.JobActions;
using CronTools.Common.Models;
using CronTools.Common.Providers;
using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Extensions;
using Rn.NetCore.Common.Factories;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;
using Rn.NetCore.Common.Services;

namespace CronTools.Common.Services
{
  public interface ICronRunnerService
  {
    Task RunCrons(string[] args);
  }

  public class CronRunnerService :BaseService<CronRunnerService>, ICronRunnerService
  {
    private readonly ILoggerAdapter<CronRunnerService> _logger;
    private readonly IDirectoryAbstraction _directory;
    private readonly IDirectoryInfoFactory _directoryInfoFactory;
    private readonly IFileAbstraction _file;
    private readonly IJsonHelper _jsonHelper;
    private readonly List<IJobAction> _jobActions;
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
      : base(serviceProvider)
    {
      // TODO: [TESTS] (CronRunnerService) Add tests
      _logger = logger;
      _directory = directory;
      _directoryInfoFactory = directoryInfoFactory;
      _file = file;
      _jsonHelper = jsonHelper;

      _config = configProvider.GetConfig();
      _jobActions = serviceProvider.GetServices<IJobAction>().ToList();
      _jobFiles = new Dictionary<string, string>();

      RefreshJobs();
    }

    public async Task RunCrons(string[] args)
    {
      // TODO: [TESTS] (CronRunnerService.RunCrons) Add tests
      if (args.Length == 0)
      {
        _logger.Warning("No jobs to run");
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
            _logger.Error("Unable to resolve action {name} for job {job}",
              step.Action.ToString("G"),
              config.Name
            );

            throw new Exception("Unable to continue");
          }

          var stepContext = new RunningStepContext(coreJobInfo, step, stepNumber++);
          if (!ValidateStepArgs(resolvedAction, stepContext))
            continue;

          var outcome = await resolvedAction.ExecuteAsync(stepContext);
          if (outcome.Succeeded)
            continue;

          _logger.Error("Step failed, stopping job");
          continueRunningSteps = false;
        }
      }

      Console.WriteLine("");
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
      var requiredArgs = action.Args
        .Where(x => x.Required)
        .ToList();

      if (requiredArgs.Count == 0)
        return true;

      foreach (var requiredArg in requiredArgs)
      {
        if(context.HasArgument(requiredArg.SafeName))
          continue;

        Logger.Warning(
          "Job '{name}' is missing required argument '{arg}' " +
          "(type: {argType}) for step '{stepNumber}':'{stepType}'!",
          context.JobInfo.Name,
          requiredArg.Name,
          requiredArg.Type.ToString("G"),
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
        _logger.Warning("Requested job {key} was not found in {directory}",
          jobKey,
          _config.JobConfigDir
        );

        return null;
      }

      // Ensure that the job file still exists
      var jobFilePath = _jobFiles[jobKey];
      if (!_file.Exists(jobFilePath))
      {
        _logger.Warning("Job file {path} no longer exists", jobFilePath);
        _jobFiles.Remove(jobKey);
        return null;
      }

      // Read contents of the config file
      var rawJson = _file.ReadAllText(jobFilePath);
      var jobConfig = _jsonHelper.DeserializeObject<JobConfig>(rawJson);

      // Handle disabled jobs
      if (!jobConfig.Enabled)
      {
        _logger.Warning("Requested job {name} is disabled", jobConfig.Name);
        return null;
      }

      _logger.Info("Loaded config for {name} ({path})",
        jobConfig.Name,
        jobFilePath
      );

      return jobConfig;
    }

    private void EnsureDirectoriesExist()
    {
      // TODO: [TESTS] (CronRunnerService.EnsureDirectoriesExist) Add tests
      if (!_directory.Exists(_config.RootDir))
        _directory.CreateDirectory(_config.RootDir);

      if (!_directory.Exists(_config.JobConfigDir))
        _directory.CreateDirectory(_config.JobConfigDir);

      if (!_directory.Exists(_config.JobConfigDir))
        throw new DirectoryNotFoundException($"Unable to find: {_config.JobConfigDir}");
    }

    private void RefreshJobs()
    {
      // TODO: [TESTS] (CronRunnerService.RefreshJobs) Add tests
      EnsureDirectoriesExist();

      _logger.Info("Refreshing jobs");
      _jobFiles.Clear();

      var directoryInfo = _directoryInfoFactory.GetDirectoryInfo(_config.JobConfigDir);
      var fileInfos = directoryInfo.GetFiles("*.json");

      if (fileInfos.Length == 0)
        return;

      _logger.Info("Discovered {count} job(s) in {directory}",
        fileInfos.Length,
        _config.JobConfigDir
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
}
