using System;
using CronTools.Common.Models;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Services;

public interface IJobSchedulerService
{
  JobSchedule ScheduleNextRun(JobConfig jobConfig);
  ScheduledJob CreateInitialSchedule(JobConfig jobConfig);
  void SyncJobConfig(JobConfig job, ScheduledJob schedule);
}

public class JobSchedulerService : IJobSchedulerService
{
  private readonly ILoggerAdapter<JobSchedulerService> _logger;

  public JobSchedulerService(ILoggerAdapter<JobSchedulerService> logger)
  {
    _logger = logger;
  }

  public JobSchedule ScheduleNextRun(JobConfig jobConfig)
  {
    // TODO: [JobSchedulerService.ScheduleNextRun] (TESTS) Add tests




    Console.WriteLine();
    return null;
  }

  public ScheduledJob CreateInitialSchedule(JobConfig jobConfig)
  {
    // TODO: [JobSchedulerService.CreateInitialSchedule] (TESTS) Add tests
    _logger.LogInformation("Creating new schedule for '{job}' ({id})",
      jobConfig.Name,
      jobConfig.JobId);

    return new ScheduledJob
    {
      JobId = jobConfig.JobId,
      LastRun = DateTimeOffset.MinValue,
      NextRun = DateTimeOffset.MinValue,
      JobName = jobConfig.Name
    };
  }

  public void SyncJobConfig(JobConfig job, ScheduledJob schedule)
  {
    // TODO: [JobSchedulerService.SyncJobConfig] (TESTS) Add tests
    schedule.JobName = job.Name;

    // Handle jobs that are set to run on start
    if (job.Schedule!.RunOnStart)
      schedule.NextRun = DateTimeOffset.MinValue;
  }
}
