using CronTools.Common.Enums;

namespace CronTools.Common.Models;

public class JobActionArg
{
  public string Name { get; set; } = string.Empty;
  public bool Required { get; set; }
  public ArgType Type { get; set; } = ArgType.String;
  public object Default { get; set; }

  public JobActionArg() { }

  public JobActionArg(string name, ArgType type, bool required, object fallback)
    : this()
  {
    // TODO: [TESTS] (JobActionArg) Add tests
    Name = name;
    Type = type;
    Required = required;
    Default = fallback;
  }

  public static JobActionArg Directory(string name, bool required, string fallback = "") =>
    // TODO: [TESTS] (JobActionArg.Directory) Add tests
    new(name, ArgType.Directory, required, fallback);

  public static JobActionArg Bool(string name, bool required, bool fallback = false) =>
    // TODO: [TESTS] (JobActionArg.Bool) Add tests
    new(name, ArgType.Bool, required, fallback);

  public static JobActionArg File(string name, bool required, string fallback = "") =>
    // TODO: [TESTS] (JobActionArg.File) Add tests
    new(name, ArgType.File, required, fallback);

  public static JobActionArg String(string name, bool required, string fallback = "") =>
    // TODO: [JobActionArg.String] (TESTS) Add tests
    new(name, ArgType.String, required, fallback);
}
