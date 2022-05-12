[Home](/README.md) / [Enums](/docs/enums/README.md) / JobStepAction

# JobStepAction
Enumeration of the supported job types for a given [job action](/docs/job-actions/README.md).

| Name | Int Value | Notes |
| --- | --- | --- |
| `Unknown` | 1 | Used when `cron-tool` is unable to resolve the given step |
| `DeleteFolder` | 2 | Indicates the usage of the [DeleteFolderAction](/docs/job-actions/DeleteFolder.md) action. |
| `CopyFile` | 3 | Indicates the usage of the [CopyFileAction](/docs/job-actions/CopyFile.md) action. |
| `DeleteFile` | 4 | Indicates the usage of the [DeleteFileAction](/docs/job-actions/DeleteFile.md) action. |
| `ZipFolder` | 5 | Indicates the usage of the [ZipFolderAction](/docs/job-actions/ZipFolder.md) action. |
| `WriteTextFile` | 6 | Indicates the usage of the [WriteTextFileAction](/docs/job-actions/WriteTextFile.md) action. |
| `DeleteFiles` | 7 | Indicates the usage of the [DeleteFiles](/docs/job-actions/DeleteFiles.md) action. |
| `GetFileSize` | 8 | Indicates the usage of the [GetFileSize](/docs/job-actions/GetFileSize.md) action. |
