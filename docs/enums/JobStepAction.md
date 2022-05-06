[Home](/README.md) / [Docs](/docs/README.md) / [Enums](/docs/enums/README.md) / JobStepAction

# JobStepAction
Enumeration of the supported job types for a given [job action](/docs/job-actions/README.md).

| Name | Int Value | Notes |
| --- | --- | --- |
| `Unknown` | 1 | Used when `cron-tool` is unable to resolve the given step |
| `DeleteFolder` | 2 | Indicates the usage of the [DeleteFolderAction](/docs/job-actions/DeleteFolder.md) action. |
| `CopyFile` | 3 | Indicates the usage of the [CopyFileAction](/docs/job-actions/CopyFile.md) action. |
| `DeleteFile` | 4 | Indicates the usage of the [DeleteFileAction](/docs/job-actions/DeleteFile.md) action. |
| `ZipFolder` | 5 | Indicates the usage of the [ZipFolderAction](/docs/job-actions/ZipFolder.md) action. |
