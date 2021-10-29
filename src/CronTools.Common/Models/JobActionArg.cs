using CronTools.Common.Enums;
using Rn.NetCore.Common.Extensions;

namespace CronTools.Common.Models
{
  public class JobActionArg
  {
    public string Name { get; set; }
    public string SafeName { get; set; }
    public bool Required { get; set; }
    public ArgType Type { get; set; }

    public JobActionArg()
    {
      // TODO: [TESTS] (JobActionArg) Add tests
      Name = string.Empty;
      SafeName = string.Empty;
      Required = false;
      Type = ArgType.String;
    }

    public JobActionArg(string name, ArgType type, bool required = false)
      : this()
    {
      // TODO: [TESTS] (JobActionArg) Add tests
      Name = name;
      SafeName = name.LowerTrim();
      Type = type;
      Required = required;
    }

    public static JobActionArg Directory(string name, bool required = false)
    {
      // TODO: [TESTS] (JobActionArg.Directory) Add tests
      return new JobActionArg(name, ArgType.Directory, required);
    }

    public static JobActionArg Bool(string name, bool required = false)
    {
      // TODO: [TESTS] (JobActionArg.Bool) Add tests
      return new JobActionArg(name, ArgType.Bool, required);
    }

    public static JobActionArg File(string name, bool required = false)
    {
      // TODO: [TESTS] (JobActionArg.File) Add tests
      return new JobActionArg(name, ArgType.File, required);
    }
  }
}
