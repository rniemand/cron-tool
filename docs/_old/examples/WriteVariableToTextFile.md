[Home](/README.md) / [Examples](/docs/examples/README.md) / Writing variable to a Text File

# Writing variable to a Text File
The below example will output a string using the below format to a text file:

    Resolved variable: ${var:SampleText}

It will make use of `variable` resolution along with the [DateTime Formatter](/docs/formatters/DateTimeFormatter.md) to inject the current date into the string via the `{date:yyyyMMdd}` placeholder.

The complete job configuration is listed below:

```json
{
  "name": "DevJob",
  "id": "C3865AE6-910C-435F-BBE4-4C51BE1A0C79",
  "variables": {
    "SampleText": "This value came from a variable @ {date:yyyyMMdd}!"
  },
  "steps": [
    {
      "name": "Write data to a text file",
      "action": "WriteTextFile",
      "args": {
        "Path": "c:\\wrk\\test.txt",
        "Contents": "Resolved variable: ${var:SampleText}",
        "Overwrite": true
      }
    }
  ]
}
```

Code execution occurs in the following order:

- Parse and process all `variables` looking for any embedded [formatters](/docs/formatters/README.md).
- Resolve and execut each enabled [job step](/docs/configuration/JobStepConfig.md).
  - Process any discovered `variables` in the job arguments.
  - Execute the desired job action.