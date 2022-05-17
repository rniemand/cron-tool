using CronTools.Common.Services;
using CronTools.Common.Utils;
using Microsoft.Extensions.DependencyInjection;

//await CronToolDIContainer.ServiceProvider
//  .GetRequiredService<IJobRunnerService>()
//  .RunJobAsync("ScheduledJob");

var service = CronToolDIContainer.ServiceProvider
  .GetRequiredService<ICronToolRunnerService>();

await service.TickAsync(CancellationToken.None); // Sets "_firstTick" to FALSE
await service.TickAsync(CancellationToken.None); // Runs scheduled jobs
