Home

# CronTool
Starter documentation for `cron-tool`, you can visit the various sections below to find out more about this application.

- [Models](/docs/models/README.md) - information about the various models used in the application
- [Job Actions](/docs/job-actions/README.md) - documentation around the supported action types
- [String formatters](/docs/string-formatters/README.md) - information about the various argument string formatters
- [Enums](/docs/enums/README.md) - list of all user exposed `enums` used by `cron-tool`
- [Examples](/docs/examples/README.md) - collection of example jobs - something to get you started

## Installation & Usage
You can install `cron-tool` by running the following command:

    dotnet tool install --global Rn.CronTool --no-cache

You can update the tool using:

    dotnet tool update --global Rn.CronTool --no-cache

This will install the tool globally, you can invoke the tool like so:

    cron-tool -r "c:\cron-tool-data" -j MyJobName

This will look in the `c:\cron-tool-data` directory for a job called `MyJobName.json` and execute it.

<!--(Rn.BuildScriptHelper){
	"version": "1.0.106",
	"replace": false
}(END)-->