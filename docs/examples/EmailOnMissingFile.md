[Home](/README.md) / [Examples](/docs/examples/README.md) / Email if file is missing

# Email if file is missing
The below example will check for the presence of a specific file and send an email if it was not found.

This makes use of the [SendEmail](/docs/job-actions/SendEmail.md) and [GetFileSize](/docs/job-actions/GetFileSize.md) actions to accomplish this. The [DateTime formatter](/docs/formatters/DateTimeFormatter.md) is used to generate the expected file name that we want to check for.

## Job Configuration
The complete job configuration is listed below:

> **Note**: additional configuration is required by the [SendMail action](/docs/job-actions/SendEmail.md) for this to work.

```json
{
  "name": "EMail if file is missing",
  "variables": {
    "BackupFilePath": "\\\\192.168.0.60\\Backups\\db-mariadb\\ghost\\{date:yyyy\\\\MM\\\\yyyy-MM-dd}-ghost.zip"
  },
  "steps": [
    {
      "name": "Publish file information",
      "action": "GetFileSize",
      "id": "queryFile",
      "args": { "Path": "${var:BackupFilePath}" }
    },
    {
      "name": "Send a mail if above threshold",
      "action": "SendEmail",
      "id": "sendMail",
      "condition": {
        "expressions": [ "queryFile.fileExists != true" ]
      },
      "args": {
        "ToAddress": "email@address.com",
        "ToName": "Richard Niemand",
        "Subject": "Backup file missing",
        "Body": "Backup file ${var:BackupFilePath} was not found!"
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
- Step `queryFile` is started
  - The file path is resolved from `${var:BackupFilePath}`
  - Information about the file is published to the [job state](/docs/general/JobState.md) with the prefix `queryFile.`
- Step `sendMail` is started
  - The [job conditions](/docs/general/JobConditions.md) are evaluated to decide if this step can run
  - If the conditions are met a mail is sent using the [SendEmail](/docs/job-actions/SendEmail.md) action
- Job execution is complete
