[Home](/README.md) / [Examples](/docs/examples/README.md) / Simple State Example

# Simple State Example
The below example makes use of the ability to push and pull state from the `running jobs context`, making it possible to interact with data from a preceding step.

```json
{
  "name": "JobState",
  "variables": {
    "SampleText": "This value came from a variable @ {date:yyyy-MM-dd HH:mm:ss}!"
  },
  "steps": [
    {
      "name": "Write data to a text file",
      "action": "WriteTextFile",
      "args": {
        "Path": "c:\\wrk\\test.txt",
        "Contents": "Resolved variable: ${var:SampleText}",
        "Overwrite": true,
        "PublishAs": "myFile"
      }
    },
    {
      "name": "Make a copy of generated file",
      "action": "CopyFile",
      "args": {
        "Source": "${state:myFile}",
        "Target": "c:\\wrk\\test.copy.txt",
        "Overwrite": true,
        "PublishAs": "myFileCopy"
      }
    },
    {
      "name": "Cleanup files",
      "action": "DeleteFiles",
      "args": {
        "Paths": [
          "${state:myFile}",
          "${state:myFileCopy}"
        ]
      }
    }
  ]
}
```

This example is processed in the following order:

- Initial values for the `variables` are calculated (e.g. `${var:SampleText}`)
- The [WriteTextFile](/docs/job-actions/WriteTextFile.md) step is executed and the value for `${var:SampleText}` is written to a file.
  - The resolved file path is published as `${state:myFile}` due to the presence of the `PublishAs` argument.
- The [CopyFile](/docs/job-actions/CopyFile.md) step is executed using the resolved value of `${state:myFile}` as the source.
  - A new stae value of `${state:myFileCopy}` is published due to the presence of the `PublishAs` argument.
- The [DeleteFiles](/docs/job-actions/DeleteFiles.md) step is executed using the previously published state values for it's input
  - `${state:myFile}` is resolved to `c:\wrk\test.txt` from step 1
  - `${state:myFileCopy}` is resolved to `c:\wrk\test.copy.txt` from step 2

The end result of this action is the following:

- A new file is created for `c:\wrk\test.txt`
- This file is copied to `c:\wrk\test.copy.txt`
- Both files are removed.