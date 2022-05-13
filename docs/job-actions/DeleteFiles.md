[Home](/README.md) / [Job Actions](/docs/job-actions/README.md) / DeleteFiles

# DeleteFiles
Provided by the `DeleteFilesAction` class.

```json
{
  "enabled": true,
  "name": "Delete provided files",
  "action": "DeleteFiles",
  "args": {
    "Paths": ["...", "..."]
  }
}
```

## Supported Arguments
Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Paths` | `File[]` | required | `[]` | Array of file(s) to delete. |
