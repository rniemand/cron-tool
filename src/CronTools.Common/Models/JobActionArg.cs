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
    Name = name;
    Type = type;
    Required = required;
    Default = fallback;
  }

  public static JobActionArg Directory(string name, bool required, string fallback = "") =>
    new(name, ArgType.Directory, required, fallback);

  public static JobActionArg Bool(string name, bool required, bool fallback = false) =>
    new(name, ArgType.Bool, required, fallback);

  public static JobActionArg File(string name, bool required, string fallback = "") =>
    new(name, ArgType.File, required, fallback);

  public static JobActionArg Email(string name, bool required, string fallback = "") =>
    new(name, ArgType.Email, required, fallback);

  public static JobActionArg Files(string name, bool required) =>
    new(name, ArgType.Files, required, string.Empty);

  public static JobActionArg String(string name, bool required, string fallback = "") =>
    new(name, ArgType.String, required, fallback);
}
