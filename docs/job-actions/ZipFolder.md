[Home](/README.md) / [Docs](/docs/README.md) / [Job Actions](/docs/job-actions/README.md) / ZipFolder

# ZipFolder
Provided by the `ZipFolderAction` class.

```json
{
  "enabled": true,
  "name": "Zip Satisfactory game saves",
  "action": "ZipFolder",
  "args": {
    "SourceDir": "...",
    "TargetZip": "...",
    "QuickZip": false,
    "IncludeBaseDir": true,
    "DeleteIfExists": true
  }
}
```

Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `SourceDir` | Directory | required | - | Path to the folder you would like to zip |
| `TargetZip` | File | required | - | Output path for the generated zip file |
| `QuickZip` | Bool | optional | `false` | When `true` will zip using store mode |
| `IncludeBaseDir` | Bool | optional | `true` | Used to include the base directory when creating your zip file |
| `DeleteIfExists` | Bool | optional | `false` | When set to `true` will delete the target zip file if it already exists |