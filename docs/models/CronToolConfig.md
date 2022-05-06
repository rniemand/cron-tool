[Home](/README.md) / [Docs](/docs/README.md) / [Models](/docs/models/README.md) / CronToolConfig

# CronToolConfig
Main application configuration.

## Root level configuration
Found at `{ "CronTool": {...} }`.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `RootDir` | Path | optional | `./config` | Root directory ro use when resolving application configuration |
| `DirectorySeparator` | string | optional | `\` | Path seperation char to use when building file & directory pathing |
| `JobConfigDir` | Path | `{root}jobs` | optional | Path to directory containing your job files |

