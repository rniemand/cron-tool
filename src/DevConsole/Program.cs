// See https://aka.ms/new-console-template for more information

using CronTools.Common.Services;
using Microsoft.Extensions.DependencyInjection;

var cronRunnerService = DIContainer.ServiceProvider.GetRequiredService<ICronRunnerService>();

await cronRunnerService.RunAsync(new[] { "DevJob" });
