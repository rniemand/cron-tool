[Home](/README.md) / [Job Actions](/docs/job-actions/README.md) / CopyFile

# CopyFile
Provided by the `CopyFileAction` class.

```json
{
  "enabled": true,
  "name": "Backup my important file",
  "action": "CopyFile",
  "args": {
    "Source": "...",
    "Target": "...",
    "Overwrite": true,
    "PublishAs": "sampleFile"
  }
}
```

Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Source` | File | required | - | Path to the source file |
| `Target` | File | required | - | Path to copy the source file to |
| `Overwrite` | Bool | optional | `false` | Overwrite target file (if exists) |
| `PublishAs` | `string` | optional | - | When set, and file was copied, this is the name of the published file path. |