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

## Example Jobs
More to come...

- [Zipping the contents of a folder](/docs/examples/BackupNasLandingPage.md) - this example covers using the [ZipFolder](/docs/job-actions/ZipFolder.md) job action.
- [Writing variable to a text file](/docs/examples/WriteVariableToTextFile.md) - this example showcases using `variables` and the [WriteTextFile](/docs/job-actions/WriteTextFile.md) job action.
- [Simple state example](/docs/examples/SimpleStateExample.md) - creates and copies a file using state to store the paths, once created the files are deleted from state.

# Development Section
Collection of developer related documentation can be found below.

## Enums
More to come...

- [Zipping the contents of a folder](/docs/examples/BackupNasLandingPage.md) - this example covers using the [ZipFolder](/docs/job-actions/ZipFolder.md) job action.
- [Writing variable to a text file](/docs/examples/WriteVariableToTextFile.md) - this example showcases using `variables` and the [WriteTextFile](/docs/job-actions/WriteTextFile.md) job action.
- [Simple state example](/docs/examples/SimpleStateExample.md) - creates and copies a file using state to store the paths, once created the files are deleted from state.
- [Simple conditions example](/docs/examples/SimpleConditions.md) - covers the basic usage of `conditions`.
- [Send Email if file is missing](/docs/examples/EmailOnMissingFile.md) - this example demonstrates sending an email if a specific file was not found.

## Job Actions
More to come...

- [CopyFile](/docs/job-actions/CopyFile.md)
- [DeleteFolder](/docs/job-actions/DeleteFolder.md)
- [DeleteFile](/docs/job-actions/DeleteFile.md)
- [DeleteFiles](/docs/job-actions/DeleteFiles.md)
- [GetFileSize](/docs/job-actions/GetFileSize.md)
- [SendEmailAction](/docs/job-actions/SendEmail.md)
- [WriteTextFile](/docs/job-actions/WriteTextFile.md)
- [ZipFolder](/docs/job-actions/ZipFolder.md)

## Configuration
More to come...

- [appsettings.json](/docs/configuration/appsettings.md)
- [JobConfig](/docs/configuration/JobConfig.md)
- [JobStepConfig](/docs/configuration/JobStepConfig.md)
- [JobStepCondition](/docs/configuration/JobStepCondition.md)
- [Globals](/docs/configuration/globals.md)

## Formatters
More to come...

- [DateTimeFormatter](/docs/formatters/DateTimeFormatter.md)

## General
General documentation for the project.

- [Job argument processing](/docs/general/ArgProcessing.md) - documents the order of precedence used when processing job arguments.
- [Configuring mail](/docs/general/ConfiguringMail.md) - covers the basics of configuring mail.
- [Job State](/docs/general/JobState.md) - explains the concept of `job state`
- [Variables](/docs/general/Variables.md) - explains the concept of `job variables`
- [Global variables](/docs/general/GlobalVariables.md) - explains the concept of `global variables`
- [Job conditions](/docs/general/JobConditions.md) - explains how `job conditions` work

<!--(Rn.BuildScriptHelper){
	"version": "1.0.106",
	"replace": false
}(END)-->