using CronTools.Common.Services;
using CronTools.Common.Utils;
using Microsoft.Extensions.DependencyInjection;

var cronRunnerService = CronToolDIContainer.ServiceProvider.GetRequiredService<ICronRunnerService>();


Console.WriteLine();
