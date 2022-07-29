# Configuration

/ [README](/README.md) / Configuration

## Overview

Holder.

- CronTool.Base.json
- CronTool.json
- CronTool.`{configuration}`.json
- CronTool.Local.json

### Configuration Layering

Holder.

```cs
var config = new ConfigurationBuilder()
  .SetBasePath(Directory.GetCurrentDirectory())
  .AddJsonFile("CronTool.Base.json")
  .AppendConfigLayer("CronTool.json")
  .AppendConfigLayer($"CronTool.{configuration}.json")
  .AppendConfigLayer("CronTool.Local.json")
  .Build();
```

where:

```cs
private static IConfigurationBuilder AppendConfigLayer(this IConfigurationBuilder builder, string file)
{
  foreach (var basePath in BasePaths)
    builder.AddJsonFile(Path.Join(basePath, file), optional: true);
  return builder;
}
```

and:

```cs
private static readonly string[] BasePaths = {
  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
  Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
  Environment.CurrentDirectory
};
```
