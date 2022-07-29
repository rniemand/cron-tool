using CronTools.Common.Models;

namespace CronTools.Common.T1.Tests.TestSupport.Builders;

public class JobConfigBuilder
{
  private readonly JobConfig _jobConfig;

  public JobConfigBuilder()
  {
    _jobConfig = new JobConfig();
  }

  public JobConfig Build() => _jobConfig;
}
