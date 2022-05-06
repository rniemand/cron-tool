using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CronTools.Common.Config;
using CronTools.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Providers;

public interface IJobConfigProvider
{
  JobConfig Resolve(string jobName);
}

public class JobConfigProvider : IJobConfigProvider
{
  private readonly CronToolConfig _config;
  private readonly IDirectoryAbstraction _directory;
  private readonly IPathAbstraction _path;
  private readonly List<string> _jobNames;

  public JobConfigProvider(IServiceProvider serviceProvider)
  {
    _config = serviceProvider.GetRequiredService<IConfigProvider>().GetConfig();
    _directory = serviceProvider.GetRequiredService<IDirectoryAbstraction>();
    _path = serviceProvider.GetRequiredService<IPathAbstraction>();

    _jobNames = DiscoverJobs();
  }

  public JobConfig Resolve(string jobName)
  {
    // TODO: [JobConfigProvider.Resolve] (TESTS) Add tests

    Console.WriteLine();


    //if (string.IsNullOrWhiteSpace(jobName))
    //  return null;

    //// Check to see if we have discovered the requested job
    //var jobKey = jobName.LowerTrim();
    //if (!_jobFiles.ContainsKey(jobKey))
    //{
    //  _logger.LogWarning("Requested job {key} was not found in {directory}",
    //    jobKey,
    //    _config.JobsDirectory
    //  );

    //  return null;
    //}

    //// Ensure that the job file still exists
    //var jobFilePath = _jobFiles[jobKey];
    //if (!_file.Exists(jobFilePath))
    //{
    //  _logger.LogWarning("Job file {path} no longer exists", jobFilePath);
    //  _jobFiles.Remove(jobKey);
    //  return null;
    //}

    //// Read contents of the config file
    //var rawJson = _file.ReadAllText(jobFilePath);
    //var jobConfig = _jsonHelper.DeserializeObject<JobConfig>(rawJson);

    //// Handle disabled jobs
    //if (!jobConfig.Enabled)
    //{
    //  _logger.LogWarning("Requested job {name} is disabled", jobConfig.Name);
    //  return null;
    //}

    //_logger.LogInformation("Loaded config for {name} ({path})",
    //  jobConfig.Name,
    //  jobFilePath
    //);

    //return jobConfig;


    return null;
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
}
