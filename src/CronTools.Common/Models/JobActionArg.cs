using CronTools.Common.Enums;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models;

public class JobActionArg
{
  public string Name { get; set; }
  public string SafeName { get; set; }
  public bool Required { get; set; }
  public ArgType Type { get; set; }
  public object Default { get; set; }

  public JobActionArg()
  {
    // TODO: [TESTS] (JobActionArg) Add tests
    Name = string.Empty;
    SafeName = string.Empty;
    Required = false;
    Type = ArgType.String;
    Default = default;
  }

  public JobActionArg(string name, ArgType type, bool required, object fallback)
    : this()
  {
    // TODO: [TESTS] (JobActionArg) Add tests
    Name = name;
    SafeName = name.LowerTrim();
    Type = type;
    Required = required;
    Default = fallback;
  }

  public static JobActionArg Directory(string name, bool required, string fallback = "")
  {
    // TODO: [TESTS] (JobActionArg.Directory) Add tests
    return new JobActionArg(name, ArgType.Directory, required, fallback);
  }

  public static JobActionArg Bool(string name, bool required, bool fallback = false)
  {
    // TODO: [TESTS] (JobActionArg.Bool) Add tests
    return new JobActionArg(name, ArgType.Bool, required, fallback);
  }

  public static JobActionArg File(string name, bool required, string fallback = "")
  {
    // TODO: [TESTS] (JobActionArg.File) Add tests
    return new JobActionArg(name, ArgType.File, required, fallback);
  }
}