[Home](/README.md) / [Job Actions](/docs/job-actions/README.md) / DeleteFolder

# DeleteFolder
Provided by the `DeleteFolderAction` class.

```json
{
  "enabled": true,
  "name": "Delete old directory",
  "action": "DeleteFolder",
  "args": {
    "Path": "...",
    "Recurse": true
  }
}
```

## Supported Arguments
Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Path` | Directory | required | - | The path of the folder you wish to delete |
| `Recurse` | Bool | optional | `false` | Recurse down deletion mode |
