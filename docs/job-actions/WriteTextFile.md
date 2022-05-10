[Home](/README.md) / [Job Actions](/docs/job-actions/README.md) / WriteTextFile

# WriteTextFile
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

More to come...

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `Path` | `Path` | required | - | Path to the text file. |
| `Contents` | `string` | required | - | Content to write to the target file. |
| `Overwrite` | `bool` | optional | `false` | How to handle an existing target file. |
