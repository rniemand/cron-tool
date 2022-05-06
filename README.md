Home

# CronTool
Starter documentation for `cron-tool`, you can visit the various sections below to find out more about this application.

- [Models](/docs/models/README.md) - information about the various models used in the application
- [Job Actions](/docs/job-actions/README.md) - documentation around the supported action types
- [String formatters](/docs/string-formatters/README.md) - information about the various argument string formatters
- [Enums](/docs/enums/README.md) - list of all user exposed `enums` used by `cron-tool`
- [Examples](/docs/examples/README.md) - collection of example jobs - something to get you started

## Basic Usage
While the `cron-tool` is in development you can use it by building the `CronTool.csproj` file and running it like below.

```shell
CronTool.exe <JobFileNameWithoutExtension>
```

This will launch the `cron-tool` and execute the given project file.

<!--(Rn.BuildScriptHelper){
	"version": "1.0.106",
	"replace": false
}(END)-->