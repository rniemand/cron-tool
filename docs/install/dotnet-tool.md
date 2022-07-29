# dotnet tool

/ [README](/README.md) / [Installation](/docs/install/README.md) / dotnet tool

## Process

You can install `cron-tool` by running the following command:

```shell
dotnet tool install --global Rn.CronTool --no-cache
dotnet tool update --global Rn.CronTool --no-cache
```

This will install the tool globally, you can invoke the tool like so:

```shell
cron-tool -r "c:\cron-tool-data" -j MyJobName
```

This will look in the `c:\cron-tool-data` directory for a job called `MyJobName.json` and execute it.
