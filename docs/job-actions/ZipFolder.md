[Home](/README.md) / [Docs](/docs/README.md) / [Job Actions](/docs/job-actions/README.md) / ZipFolder

# ZipFolder
Provided by the `ZipFolderAction` class.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `SourceDir` | Path | required | - | Path to the folder you would like to zip |
| `TargetZip` | Path | required | - | Output path for the generated zip file |
| `Quick` | bool | optional | `false` | When `true` will zip using store mode |
| `IncludeBaseDirectory` | bool | optional | `true` | Used to include the base directory when creating your zip file |
| `DeleteTargetZipIfExists` | bool | optional | `false` | When set to `true` will delete the target zip file if it already exists |