using System;
using System.Threading.Tasks;
using CronTools.Common.Config;
using CronTools.Common.Providers;
using Rn.NetCore.Common.Services;

namespace CronTools.Common.Services
{
  public interface ICronRunnerService
  {
    Task RunCrons(string[] args);
  }

  public class CronRunnerService :BaseService<CronRunnerService>, ICronRunnerService
  {
    private readonly CronToolConfig _config;

    public CronRunnerService(
      IServiceProvider serviceProvider,
      IConfigProvider configProvider)
      : base(serviceProvider)
    {
      // TODO: [TESTS] (CronRunnerService) Add tests
      _config = configProvider.GetConfig();
    }

    public async Task RunCrons(string[] args)
    {
      // TODO: [TESTS] (CronRunnerService.RunCrons) Add tests




      Console.WriteLine("");
    }
  }
}
