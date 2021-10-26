using System;
using System.IO;
using System.Threading.Tasks;
using CronTools.Common.Config;
using CronTools.Common.Providers;
using Rn.NetCore.Common.Abstractions;
using Rn.NetCore.Common.Services;

namespace CronTools.Common.Services
{
  public interface ICronRunnerService
  {
    Task RunCrons(string[] args);
  }

  public class CronRunnerService :BaseService<CronRunnerService>, ICronRunnerService
  {
    private readonly IDirectoryAbstraction _directory;
    private readonly CronToolConfig _config;

    public CronRunnerService(
      IServiceProvider serviceProvider,
      IConfigProvider configProvider,
      IDirectoryAbstraction directory)
      : base(serviceProvider)
    {
      _directory = directory;
      // TODO: [TESTS] (CronRunnerService) Add tests
      _config = configProvider.GetConfig();
    }

    public async Task RunCrons(string[] args)
    {
      // TODO: [TESTS] (CronRunnerService.RunCrons) Add tests
      EnsureDirectoriesExist();



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
  }
}
