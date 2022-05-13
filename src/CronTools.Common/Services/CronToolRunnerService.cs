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

    // Check to see if there are any jobs that need to run now
    var jobIds = GetRunnableJobIds();
    if (jobIds.Count == 0)
      return;

    // Run each scheduled job and reschedule
    foreach (var jobId in jobIds)
    {
      if (stoppingToken.IsCancellationRequested)
        return;

      var resolvedJob = _enabledJobs.FirstOrDefault(x => x.JobId.IgnoreCaseEquals(jobId));
      if (resolvedJob is null)
      {
        RemoveMissingJob(jobId);
        continue;
      }

      await RunScheduledJob(jobId, resolvedJob);
    }


    await Task.CompletedTask;
  }

  private async Task RunScheduledJob(string jobKey, JobConfig jobConfig)
  {
    // TODO: [CronToolRunnerService.RunScheduledJob] (TESTS) Add tests





    Console.WriteLine();
    Console.WriteLine();
  }

  // Working with scheduled jobs
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

    if (_scheduledJobs.ContainsKey(jobKey))
    {
      _scheduledJobs[jobKey].JobName = jobConfig.Name;
      return;
    }

    _logger.LogInformation("Creating new schedule for '{job}' ({id})", jobConfig.Name, jobConfig.JobId);
    _scheduledJobs[jobKey] = new ScheduledJob
    {
      JobId = jobConfig.JobId,
      LastRun = DateTimeOffset.MinValue,
      NextRun = DateTimeOffset.MinValue,
      JobName = jobConfig.Name
    };
  }

  private void RunJobIntegrityCheck()
  {
    // TODO: [CronToolRunnerService.RunJobIntegrityCheck] (TESTS) Add tests
    var staleScheduleKeys = new List<string>();

    foreach (var scheduledJob in _scheduledJobs)
    {
      var scheduledJobKey = scheduledJob.Key;
      if (_enabledJobs.Any(x => x.JobId.IgnoreCaseEquals(scheduledJobKey)))
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

  private List<string> GetRunnableJobIds()
  {
    // TODO: [CronToolRunnerService.GetRunnableJobIds] (TESTS) Add tests
    if (_scheduledJobs.Count == 0)
      return new List<string>();

    var jobIds = new List<string>();
    var dateThreshold = _dateTime.Now;
    foreach (var scheduledJob in _scheduledJobs)
    {
      if (scheduledJob.Value.NextRun > dateThreshold)
        continue;

      _logger.LogDebug("Job '{name}' ({id}) is scheduled to run now",
        scheduledJob.Value.JobName,
        scheduledJob.Value.JobId);

      jobIds.Add(scheduledJob.Key);
    }

    if (jobIds.Count == 0)
      return new List<string>();

    _logger.LogInformation("Found {count} jobs that need to run", jobIds.Count);
    return jobIds;
  }

  private void RemoveMissingJob(string jobId)
  {
    // TODO: [CronToolRunnerService.RemoveMissingJob] (TESTS) Add tests
    if (!_scheduledJobs.Any(x => x.Key.IgnoreCaseEquals(jobId)))
      return;

    var resolvedJobKey = _scheduledJobs
      .First(x => x.Key.IgnoreCaseEquals(jobId))
      .Key;

    _logger.LogDebug("Removing missing job key: {jobId}", resolvedJobKey);
    _scheduledJobs.Remove(resolvedJobKey);
  }
}
