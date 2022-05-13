[Home](/README.md) / [Examples](/docs/examples/README.md) / Simple Conditions

# Simple Conditions
The below example covers the basic usage of [job step](/docs/configuration/JobStepConfig.md
) `conditions` ([more here](/docs/configuration/JobStepCondition.md)) with the `cron-tool`.

Additionaly this job makes use of [variables](/docs/general/Variables.md), [job state](/docs/general/JobState.md) and [formatters](/docs/formatters/README.md) to accomplish the task at hand.

## Job Goal
The goal of this job is to check for the existance (and size) of a daily generated backup file, should the file be above a set threshold of `30000 bytes` it will write some content to a text file.

The backups live in a directory on my NAS that follows this naming convention:

    \\192.168.0.60\Backups\db-mariadb\ghost\<year>\<month>\<year>-<month>-<day>-ghost.zip

With the use of the [DateTime formatter](/docs/formatters/DateTimeFormatter.md) we can generate the folder structure using the following expression.

    \\192.168.0.60\Backups\db-mariadb\ghost\{date:yyyy\\MM\\yyyy-MM-dd}-ghost.zip

> **Note**: You need to escape `\` within the `{date"xx}` expression.

## Job Configuration
The configuration for this job is pretty simple once you understand the basics:

```json
{
  "name": "Conditions Example",
  "variables": {
    "BackupFilePath": "\\\\192.168.0.60\\Backups\\db-mariadb\\ghost\\{date:yyyy\\\\MM\\\\yyyy-MM-dd}-ghost.zip",
    "ThresholdBackupSize": 30000
  },
  "steps": [
    {
      "name": "Get size of latest DB backup",
      "action": "GetFileSize",
      "id": "backupFile",
      "args": {
        "Path": "${var:BackupFilePath}"
      }
    },
    {
      "name": "Conditionally write to a text file",
      "action": "WriteTextFile",
      "id": "writeFile",
      "condition": {
        "type": "and",
        "stopOnFailure": true,
        "expressions": [
          "backupFile.fileExists = true",
          "backupFile.fileSize > ${var:ThresholdBackupSize}"
        ]
      },
      "args": {
        "Path": "c:\\wrk\\test.txt",
        "Contents": "Backup file was ${state:backupFile.fileSize} bytes at {date:yyyy-MM-dd hh:mm:ss} | ${global:test}",
        "Overwrite": true
      }
    }
  ]
}
```

## Flow Breakdown
The job is processed in the following manner.

- [Global Variables](/docs/general/GlobalVariables.md) are evaluated and added to the running job context
- [Job level variables](/docs/general/Variables.md) are processed and attached to the running job context
  - The [DateTime formatter](/docs/formatters/DateTimeFormatter.md) is used to generate a value for `BackupFilePath`
- Step `backupFile` sis started
  - The [GetFileSize](/docs/job-actions/GetFileSize.md) action is executed
  - The file name is resolved from `${var:BackupFilePath}`
  - Information about the file is tracked in the [job state](/docs/general/JobState.md) prefixed wit the jobId `backupFile`
- Step `writeFile` is started
  - The [job conditions](/docs/general/JobConditions.md) are evaluated to decide if this step can run
  - If conditions pass, data is written to `c:\wrk\test.txt`
- Job execution is complete
