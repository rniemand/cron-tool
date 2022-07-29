using CronTools.Common.Models;

namespace CronTools.Common.T1.Tests.TestSupport.Builders;

public class RunningJobContextBuilder
{
  private readonly RunningJobContext _context;

  public RunningJobContextBuilder()
  {
    _context = new RunningJobContext(new JobConfig());
  }

  public RunningJobContextBuilder WithStateValue(string key, object value)
  {
    _context.State[key] = value;
    return this;
  }

  public RunningJobContextBuilder WithVariable(string key, object value)
  {
    _context.Variables[key] = value;
    return this;
  }

  public RunningJobContext Build() => _context;
}
