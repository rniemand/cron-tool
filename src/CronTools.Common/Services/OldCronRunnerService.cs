using System.Threading.Tasks;
using CronTools.Common.Providers;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Services;

public interface IOldCronRunnerService
{
  Task RunJobsAsync(string[] args);
}

public class OldCronRunnerService : IOldCronRunnerService
{
  private readonly ILoggerAdapter<OldCronRunnerService> _logger;
  private readonly IJobConfigProvider _jobConfigProvider;
  private readonly IJobRunnerService _jobRunnerService;

  public OldCronRunnerService(
    ILoggerAdapter<OldCronRunnerService> logger,
    IJobConfigProvider jobConfigProvider,
    IJobRunnerService jobRunnerService)
  {
    _logger = logger;
    _jobConfigProvider = jobConfigProvider;
    _jobRunnerService = jobRunnerService;
  }

  public async Task RunJobsAsync(string[] args)
  {
    // TODO: [TESTS] (CronRunnerService.RunJobsAsync) Add tests
    if (args.Length == 0)
    {
      _logger.LogWarning("No jobs to run");
      return;
    }

    foreach (var jobName in args)
    {
      var jobConfig = _jobConfigProvider.Resolve(jobName);
      if (jobConfig is null)
        continue;

      await _jobRunnerService.RunJobAsync(jobConfig);
    }
  }
}
