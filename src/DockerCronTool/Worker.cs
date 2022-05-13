using CronTools.Common.Services;

namespace DockerCronTool;

public class Worker : BackgroundService
{
  private readonly ICronToolRunnerService _runnerService;


  public Worker(ICronToolRunnerService runnerService)
  {
    _runnerService = runnerService;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      await _runnerService.TickAsync(stoppingToken);
      await Task.Delay(1000, stoppingToken);
    }
  }
}
