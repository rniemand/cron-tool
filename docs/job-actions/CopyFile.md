[Home](/README.md) / [Docs](/docs/README.md) / [Job Actions](/docs/job-actions/README.md) / CopyFile

# CopyFile
Provided by the `CopyFileAction` class.

```json
{
  "enabled": true,
  "name": "Backup my important file",
  "action": "CopyFile",
  "args": {
    "Source": "...",
    "Destination": "...",
    "Overwrite": true
  }
}
```

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Source` | File | required | - | Path to the source file |
| `Destination` | File | required | - | Path to copy the source file to |
| `Overwrite` | Bool | optional | `false` | Overwrite target file (if exists) |