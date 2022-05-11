[Home](/README.md) / [Job Actions](/docs/job-actions/README.md) / WriteTextFile

# WriteTextFile
Writes the provided contents to a text file.

Provided by the `WriteTextFileAction` class.

```json
{
  "enabled": true,
  "name": "Write data to a text file",
  "action": "WriteTextFile",
  "args": {
    "Path": "...",
    "Contents": "File contents",
    "Overwrite": true
  }
}
```

Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Path` | `Path` | required | - | Path to the text file. |
| `Contents` | `string` | required | - | Content to write to the target file. |
| `Overwrite` | `bool` | optional | `false` | How to handle an existing target file. |
