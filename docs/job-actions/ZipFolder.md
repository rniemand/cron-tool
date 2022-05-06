[Home](/README.md) / [Docs](/docs/README.md) / [Job Actions](/docs/job-actions/README.md) / ZipFolder

# ZipFolder
Provided by the `ZipFolderAction` class.

```json
{
  "enabled": true,
  "name": "Zip Satisfactory saves",
  "action": "ZipFolder",
  "args": {
    "SourceDir": "...",
    "TargetZip": "...",
    "Quick": true,
    "IncludeBaseDirectory": true,
    "DeleteTargetZipIfExists": true
  }
}
```

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `SourceDir` | Directory | required | - | Path to the folder you would like to zip |
| `TargetZip` | File | required | - | Output path for the generated zip file |
| `Quick` | Bool | optional | `false` | When `true` will zip using store mode |
| `IncludeBaseDirectory` | Bool | optional | `true` | Used to include the base directory when creating your zip file |
| `DeleteTargetZipIfExists` | Bool | optional | `false` | When set to `true` will delete the target zip file if it already exists |