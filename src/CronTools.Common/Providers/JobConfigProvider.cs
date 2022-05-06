using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CronTools.Common.Config;
using CronTools.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Extensions;
using Rn.NetCore.Common.Helpers;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Providers;

public interface IJobConfigProvider
{
  JobConfig Resolve(string jobName);
}

public class JobConfigProvider : IJobConfigProvider
{
  private readonly CronToolConfig _config;
  private readonly ILoggerAdapter<JobConfigProvider> _logger;
  private readonly IDirectoryAbstraction _directory;
  private readonly IFileAbstraction _file;
  private readonly IPathAbstraction _path;
  private readonly IJsonHelper _jsonHelper;
  private readonly List<string> _jobNames;

  public JobConfigProvider(IServiceProvider serviceProvider)
  {
    _logger = serviceProvider.GetRequiredService<ILoggerAdapter<JobConfigProvider>>();
    _directory = serviceProvider.GetRequiredService<IDirectoryAbstraction>();
    _path = serviceProvider.GetRequiredService<IPathAbstraction>();
    _file = serviceProvider.GetRequiredService<IFileAbstraction>();
    _jsonHelper = serviceProvider.GetRequiredService<IJsonHelper>();
    _config = serviceProvider.GetRequiredService<IConfigProvider>().GetConfig();

    _jobNames = DiscoverJobs();
  }


  public JobConfig Resolve(string jobName)
  {
    // TODO: [JobConfigProvider.Resolve] (TESTS) Add tests
    if (string.IsNullOrWhiteSpace(jobName))
      return null;

    // Check to see if we have discovered the requested job
    if (!JobExists(jobName))
    {
      _logger.LogWarning("No job with name '{name}' found in: {path}", jobName, _config.JobsDirectory);
      return null;
    }

    // Ensure that the job file still exists
    var jobFilePath = _path.Join(_config.JobsDirectory, $"{jobName}.json");
    if (!_file.Exists(jobFilePath))
    {
      _logger.LogWarning("Unable to find job file: {path}", jobFilePath);
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
      jobFilePath);

    return jobConfig;
  }


  private void EnsureDirectoryExists(string path)
  {
    // TODO: [JobConfigProvider.EnsureDirectoryExists] (TESTS) Add tests
    if(_directory.Exists(path))
      return;

    _directory.CreateDirectory(path);
  }

  private List<string> DiscoverJobs()
  {
    // TODO: [JobConfigProvider.DiscoverJobs] (TESTS) Add tests
    EnsureDirectoryExists(_config.JobsDirectory);
    
    var jsonFiles = _directory.GetFiles(_config.JobsDirectory,
      "*.json",
      SearchOption.TopDirectoryOnly);
    
    return jsonFiles
      .Select(jsonFile => _path.GetFileNameWithoutExtension(jsonFile))
      .ToList();
  }

  private bool JobExists(string jobName) =>
    // TODO: [JobConfigProvider.JobExists] (TESTS) Add tests
    _jobNames.Any(x => x.IgnoreCaseEquals(jobName));
}
