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

Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Path` | `Path` | required | - | Path to the file that you want to query size information for. |
