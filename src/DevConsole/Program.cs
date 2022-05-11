using CronTools.Common.Services;
using Microsoft.Extensions.DependencyInjection;

var cronRunnerService = DIContainer.ServiceProvider.GetRequiredService<ICronRunnerService>();

await cronRunnerService.RunAsync(new[] { "JobState" });
