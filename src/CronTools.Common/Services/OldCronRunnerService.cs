using System.Threading.Tasks;
using Rn.NetCore.Common.Logging;

namespace CronTools.Common.Services;

public interface IOldCronRunnerService
{
  Task RunJobsAsync(string[] args);
}

public class OldCronRunnerService : IOldCronRunnerService
{
  private readonly ILoggerAdapter<OldCronRunnerService> _logger;
  private readonly IJobRunnerService _jobRunnerService;

  public OldCronRunnerService(
    ILoggerAdapter<OldCronRunnerService> logger,
    IJobRunnerService jobRunnerService)
  {
    _logger = logger;
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
      await _jobRunnerService.RunJobAsync(jobName);
    }
  }
}
