# CronTool (cron-tool)
Starter documentation for `cron-tool`, you can visit the various sections below to find out more about this application.

## Installation & Usage
You can install `cron-tool` by running the following command:

    dotnet tool install --global Rn.CronTool --no-cache

You can update the tool using:

    dotnet tool update --global Rn.CronTool --no-cache

This will install the tool globally, you can invoke the tool like so:

    cron-tool -r "c:\cron-tool-data" -j MyJobName

This will look in the `c:\cron-tool-data` directory for a job called `MyJobName.json` and execute it.

# Development Section
Collection of developer related documentation can be found below.

## Enums
More to come...

- [ArgType](/docs/enums/ArgType.md) - documents the type of arguments exposed by the various [job actions](/docs/job-actions/README.md).
- [JobStepAction](/docs/enums/JobStepAction.md) - list of all supported [job actions](/docs/job-actions/README.md).

## Job Actions
More to come...

- [CopyFile](/docs/job-actions/CopyFile.md)
- [DeleteFolder](/docs/job-actions/DeleteFolder.md)
- [DeleteFile](/docs/job-actions/DeleteFile.md)
- [WriteTextFile](/docs/job-actions/WriteTextFile.md)
- [ZipFolder](/docs/job-actions/ZipFolder.md)

## Configuration
More to come...

- [appsettings.json](/docs/configuration/appsettings.md)
- [JobConfig](/docs/configuration/JobConfig.md)
- [JobStepConfig](/docs/configuration/JobStepConfig.md)

## Formatters
More to come...

- [DateTimeFormatter](/docs/formatters/DateTimeFormatter.md)

<!--(Rn.BuildScriptHelper){
	"version": "1.0.106",
	"replace": false
}(END)-->