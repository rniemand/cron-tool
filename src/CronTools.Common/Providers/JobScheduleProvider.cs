using System;
using System.Collections.Generic;
using CronTools.Common.Models;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Providers;

public interface IJobScheduleProvider
{
  Dictionary<string, ScheduledJob> LoadSchedule();
  void SaveSchedule(Dictionary<string, ScheduledJob> scheduledJobs);
}

public class JobScheduleProvider : IJobScheduleProvider
{
  private readonly ILoggerAdapter<JobScheduleProvider> _logger;
  private readonly IJsonHelper _jsonHelper;
  private readonly IFileAbstraction _file;
  private readonly IDirectoryAbstraction _directory;
  private readonly IPathAbstraction _path;
  private readonly string _scheduleFile;

  public JobScheduleProvider(
    ILoggerAdapter<JobScheduleProvider> logger,
    IConfigProvider configProvider,
    IPathAbstraction pathAbstraction,
    IJsonHelper jsonHelper,
    IFileAbstraction file,
    IDirectoryAbstraction directory)
  {
    _logger = logger;
    _jsonHelper = jsonHelper;
    _file = file;
    _directory = directory;
    _path = pathAbstraction;

    var config = configProvider.GetConfig();
    _scheduleFile = pathAbstraction.Join(config.JobsDirectory, "_schedules.json");
  }

  public Dictionary<string, ScheduledJob> LoadSchedule()
  {
    EnsureScheduleFileExists();

    var rawJson = _file.ReadAllText(_scheduleFile);
    return _jsonHelper.DeserializeObject<Dictionary<string, ScheduledJob>>(rawJson);
  }

  public void SaveSchedule(Dictionary<string, ScheduledJob> scheduledJobs)
  {
    BackupSchedule();

    var scheduleJson = _jsonHelper.SerializeObject(scheduledJobs, true);
    _file.WriteAllText(_scheduleFile, scheduleJson);
  }

  private void EnsureScheduleFileExists()
  {
    var directoryName = _path.GetDirectoryName(_scheduleFile);
    if (string.IsNullOrWhiteSpace(directoryName))
    {
      _logger.LogError("Unable to determine directory name from: {path}", _scheduleFile);
      throw new Exception("Unable to determine directory path");
    }

    if (!_directory.Exists(directoryName))
      _directory.CreateDirectory(directoryName);

    if (_file.Exists(_scheduleFile))
      return;

    _logger.LogInformation("Creating initial schedule state file: {path}", _scheduleFile);
    var dummyData = new Dictionary<string, ScheduledJob>();
    var dummyJson = _jsonHelper.SerializeObject(dummyData, true);
    _file.WriteAllText(_scheduleFile, dummyJson);
  }

  private void BackupSchedule()
  {
    var backupPath = $"{_scheduleFile}.backup";
    _logger.LogDebug("Backing up schedule to: {path}", backupPath);

    if(_file.Exists(backupPath))
      _file.Delete(backupPath);

    _file.Move(_scheduleFile, backupPath);
  }
}
