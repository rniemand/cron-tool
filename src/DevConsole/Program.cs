using CronTools.Common.Services;
using CronTools.Common.Utils;
using Microsoft.Extensions.DependencyInjection;

await CronToolDIContainer.ServiceProvider
  .GetRequiredService<ICronRunnerService>()
  .RunAsync(new[] { "SendMail" });
