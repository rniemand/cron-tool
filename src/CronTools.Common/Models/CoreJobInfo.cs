namespace CronTools.Common.Models;

public class CoreJobInfo
{
  public string Name { get; set; } = string.Empty;

  public CoreJobInfo() { }

  public CoreJobInfo(string name)
  {
    Name = name;
  }
}
