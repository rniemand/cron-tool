using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CronTools.Common.Config;
using CronTools.Common.Providers;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Extensions;
using Rn.NetCore.Common.Factories;
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
    private readonly CronToolConfig _config;
    private readonly Dictionary<string, string> _jobs;

    public CronRunnerService(
      ILoggerAdapter<CronRunnerService> logger,
      IServiceProvider serviceProvider,
      IConfigProvider configProvider,
      IDirectoryAbstraction directory,
      IDirectoryInfoFactory directoryInfoFactory)
      : base(serviceProvider)
    {
      // TODO: [TESTS] (CronRunnerService) Add tests
      _logger = logger;
      _directory = directory;
      _directoryInfoFactory = directoryInfoFactory;
      _config = configProvider.GetConfig();

      _jobs = new Dictionary<string, string>();

      RefreshJobs();
    }

    public async Task RunCrons(string[] args)
    {
      // TODO: [TESTS] (CronRunnerService.RunCrons) Add tests



      

      Console.WriteLine("");
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
      _jobs.Clear();

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

        _jobs[cleanName] = fileInfo.FullName;
      }
    }
  }
}
