[Home](/README.md) / [Config](/docs/configuration/README.md) / CronToolConfig

# CronToolConfig
Main application configuration.

```json
{
  "CronTool": {
    "rootDir": "\\\\192.168.0.60\\appdata\\cron-tool",
    "dirSeparator": "\\",
    "jobsDirectory": "{root}jobs"
  } 
}
```

Below is a brekdown of each configuration value.

| Path | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `rootDir` | Path | optional | `./config` | Root directory ro use when resolving application configuration. `./` will be replaced with the application base directory. |
| `dirSeparator` | string | optional | `\` | Path seperation char to use when building file & directory pathing. |
| `jobsDirectory` | Path | optional | `{root}jobs` | Path to directory containing your job files. You can use `{root}` in this path if so desired. |
