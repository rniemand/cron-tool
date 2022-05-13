using System.Threading;
using System.Threading.Tasks;

namespace CronTools.Common.Services;

public interface ICronToolRunnerService
{
  Task TickAsync(CancellationToken stoppingToken);
}

public class CronToolRunnerService : ICronToolRunnerService
{
  public async Task TickAsync(CancellationToken stoppingToken)
  {
    // TODO: [CronToolRunnerService.TickAsync] (TESTS) Add tests


    await Task.CompletedTask;
  }
}
