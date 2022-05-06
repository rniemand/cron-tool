[Home](/README.md) / [Docs](/docs/README.md) / [Job Actions](/docs/job-actions/README.md) / DeleteFolder

# DeleteFolder
Provided by the `DeleteFolderAction` class.

```json
{
  "enabled": true,
  "name": "Delete old directory",
  "action": "DeleteFolder",
  "args": {
    "Directory": "...",
    "Recurse": true
  }
}
```

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Directory` | Directory | required | - | The path of the folder you wish to delete |
| `Recurse` | Bool | optional | `false` | Recurse down deletion mode |
