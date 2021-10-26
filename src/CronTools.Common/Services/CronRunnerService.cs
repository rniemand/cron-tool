using System;
using System.Threading.Tasks;
using Rn.NetCore.Common.Services;

namespace CronTools.Common.Services
{
  public interface ICronRunnerService
  {
    Task RunCrons(string[] args);
  }

  public class CronRunnerService :BaseService<CronRunnerService>, ICronRunnerService
  {
    public CronRunnerService(IServiceProvider serviceProvider)
      : base(serviceProvider)
    {
      // TODO: [TESTS] (CronRunnerService) Add tests
    }

    public async Task RunCrons(string[] args)
    {
      // TODO: [TESTS] (CronRunnerService.RunCrons) Add tests




      Console.WriteLine("");
    }
  }
}
