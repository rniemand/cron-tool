using System.CommandLine;
using CronTool;

await DotNetCronTool.BuildCommand().InvokeAsync(args);
