using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CronTools.Common.Models;
using CronTools.Common.Providers;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Extensions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Services;

public interface ICronToolRunnerService
{
  Task TickAsync(CancellationToken stoppingToken);
}

public class CronToolRunnerService : ICronToolRunnerService
{
  private readonly ILoggerAdapter<CronToolRunnerService> _logger;
  private readonly IJobConfigProvider _jobConfigProvider;
  private readonly IDateTimeAbstraction _dateTime;
  private readonly IJobScheduleProvider _scheduleProvider;
  private readonly Dictionary<string, ScheduledJob> _scheduledJobs;
  private List<JobConfig> _enabledJobs;
  private DateTime _nextJobRefresh;

  public CronToolRunnerService(
    ILoggerAdapter<CronToolRunnerService> logger,
    IJobConfigProvider jobConfigProvider,
    IDateTimeAbstraction dateTime,
    IJobScheduleProvider scheduleProvider)
  {
    _logger = logger;
    _jobConfigProvider = jobConfigProvider;
    _dateTime = dateTime;
    _scheduleProvider = scheduleProvider;

    _enabledJobs = new List<JobConfig>();
    _scheduledJobs = _scheduleProvider.LoadSchedule();
    _nextJobRefresh = dateTime.MinValue;
  }

  public async Task TickAsync(CancellationToken stoppingToken)
  {
    // TODO: [CronToolRunnerService.TickAsync] (TESTS) Add tests
    RefreshJobs();


    await Task.CompletedTask;
  }

  private void RefreshJobs()
  {
    // TODO: [CronToolRunnerService.RefreshJobs] (TESTS) Add tests
    if (_dateTime.Now < _nextJobRefresh)
      return;

    _enabledJobs.Clear();
    _nextJobRefresh = _dateTime.Now.AddMinutes(10);
    _logger.LogDebug("Refreshing jobs");

    _enabledJobs = _jobConfigProvider.ResolveAllEnabled();
    if (_enabledJobs.Count == 0)
      return;

    _logger.LogDebug("Discovered {count} enabled jobs", _enabledJobs.Count);
    foreach (var jobConfig in _enabledJobs)
    {
      EnsureJobScheduled(jobConfig);
    }

    RunJobIntegrityCheck();
    _scheduleProvider.SaveSchedule(_scheduledJobs);
  }

  private void EnsureJobScheduled(JobConfig jobConfig)
  {
    // TODO: [CronToolRunnerService.EnsureJobScheduled] (TESTS) Add tests
    var jobKey = jobConfig.JobId.LowerTrim();

    if(_scheduledJobs.ContainsKey(jobKey))
      return;

    _logger.LogInformation("Creating new schedule for '{job}' ({id})", jobConfig.Name, jobConfig.JobId);
    _scheduledJobs[jobKey] = new ScheduledJob
    {
      JobId = jobConfig.JobId,
      LastRun = DateTimeOffset.MinValue,
      NextRun = DateTimeOffset.MinValue
    };
  }

  private void RunJobIntegrityCheck()
  {
    // TODO: [CronToolRunnerService.RunJobIntegrityCheck] (TESTS) Add tests
    var staleScheduleKeys = new List<string>();

    foreach (var scheduledJob in _scheduledJobs)
    {
      var scheduledJobKey = scheduledJob.Key;
      if(_enabledJobs.Any(x => x.JobId.IgnoreCaseEquals(scheduledJobKey)))
        continue;

      staleScheduleKeys.Add(scheduledJobKey);
    }

    if (staleScheduleKeys.Count == 0)
      return;

    _logger.LogInformation("Removing {count} stale schedule entries", staleScheduleKeys.Count);
    foreach (var scheduleKey in staleScheduleKeys)
    {
      _scheduledJobs.Remove(scheduleKey);
    }
  }
}
