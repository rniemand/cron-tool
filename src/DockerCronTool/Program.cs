using CronTools.Common.Extensions;
using DockerCronTool;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
      services.AddCronTool();
      services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
