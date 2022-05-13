[Home](/README.md) / [Job Actions](/docs/job-actions/README.md) / DeleteFile

# DeleteFile
Provided by the `DeleteFileAction` class.

```json
{
  "enabled": true,
  "name": "Delete temp file",
  "action": "DeleteFile",
  "args": {
    "Path": "..."
  }
}
```

## Supported Arguments
Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Path` | File | required | - | The path of the file you wish to delete |