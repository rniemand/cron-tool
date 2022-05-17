using System;
using CronTools.Common.Enums;
using CronTools.Common.Helpers;
using CronTools.Common.Models;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Extensions;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Services;

public interface IJobSchedulerService
{
  ScheduledJob ScheduleNextRun(JobConfig jobConfig);
  ScheduledJob CreateInitialSchedule(JobConfig jobConfig);
  void SyncJobConfig(JobConfig job, ScheduledJob schedule);
}

public class JobSchedulerService : IJobSchedulerService
{
  private readonly ILoggerAdapter<JobSchedulerService> _logger;
  private readonly IDateTimeAbstraction _dateTime;

  public JobSchedulerService(
    ILoggerAdapter<JobSchedulerService> logger,
    IDateTimeAbstraction dateTime)
  {
    _logger = logger;
    _dateTime = dateTime;
  }

  public ScheduledJob ScheduleNextRun(JobConfig jobConfig)
  {
    // TODO: [JobSchedulerService.ScheduleNextRun] (TESTS) Add tests
    if (jobConfig.Schedule is null)
      throw new Exception("Job has no schedule");

    var jobSchedule = new ScheduledJob
    {
      JobId = jobConfig.JobId,
      JobName = jobConfig.Name,
      LastRun = _dateTime.Now,
      NextRun = GetNextRunTime(jobConfig.Schedule)
    };

    _logger.LogInformation("Scheduled '{job}' ({id}) next run for: {time}",
      jobConfig.Name,
      jobConfig.JobId,
      jobSchedule.NextRun);

    return jobSchedule;
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
      LastRun = DateTime.MinValue,
      NextRun = DateTime.MinValue,
      JobName = jobConfig.Name
    };
  }

  public void SyncJobConfig(JobConfig job, ScheduledJob schedule)
  {
    // TODO: [JobSchedulerService.SyncJobConfig] (TESTS) Add tests
    schedule.JobName = job.Name;

    // Handle jobs that are set to run on start
    if (job.Schedule!.RunOnStart)
      schedule.NextRun = DateTime.MinValue;
  }

  private DateTime GetNextRunTime(JobSchedule schedule)
  {
    // TODO: [JobSchedulerService.GetNextRunTime] (TESTS) Add tests
    switch (schedule.Frequency)
    {
      case ScheduleFrequency.TimeOfDay:
        return TimeOfDayNextRunTime(schedule);

      case ScheduleFrequency.Minute:
        return _dateTime.Now.AddMinutes(schedule.IntValue);

      case ScheduleFrequency.Hour:
        return _dateTime.Now.AddHours(schedule.IntValue);

      case ScheduleFrequency.Day:
        return _dateTime.Now.AddDays(schedule.IntValue);

      default:
        var freqName = schedule.Frequency.ToString("G");
        throw new Exception($"Add support for: {freqName}");
    }
  }

  private DateTime TimeOfDayNextRunTime(JobSchedule schedule)
  {
    if (string.IsNullOrWhiteSpace(schedule.TimeOfDay))
      throw new ArgumentException("No TimeOfDay value specified", nameof(schedule));

    if(!schedule.TimeOfDay.MatchesRegex("(\\d{2}):(\\d{2})"))
      throw new ArgumentException("Invalid TimeOfDay value specified", nameof(schedule));

    var todParts = schedule.TimeOfDay.Split(':');
    var hours = CastHelper.StringToInt(todParts[0]);
    var minuets = CastHelper.StringToInt(todParts[1]);
    var baseDate = _dateTime.Now.AddDays(1);
    return new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, hours, minuets, 0);
  }
}
