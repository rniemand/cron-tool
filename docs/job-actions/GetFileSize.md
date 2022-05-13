[Home](/README.md) / [Job Actions](/docs/job-actions/README.md) / GetFileSize

# GetFileSize
Provided by the `GetFileSizeAction` class, publishes the current file size to the jobs `state` for usage elsewhere.

```json
{
  "enabled": true,
  "name": "Delete old directory",
  "action": "GetFileSize",
  "args": {
    "Path": "..."
  }
}
```

## Supported Arguments
Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Path` | `Path` | required | - | Path to the file that you want to query size information for. |

## Published State
Below is a list of all state values published by this job action.

| Property | Type | Published | Default | Notes |
| --- | --- | --- | --- | --- |
| `{id}.fileExists` | `bool` | always | `false` | Indicates that the requested file was found |
| `{id}.fileSize` | `long` | file exists | `0` | The length of the file in `bytes`. |
| `{id}.filePath` | `string` | file exists | - | Resolved path to the file. |
