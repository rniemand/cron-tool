namespace CronTools.Common.Enums;

// DOCS: docs\enums\JobStepAction.md
public enum JobStepAction
{
  Unknown = 1,
  DeleteFolder = 2,
  CopyFile = 3,
  DeleteFile = 4,
  ZipFolder = 5,
  WriteTextFile = 6,
  DeleteFiles = 7,
  GetFileSize = 8,
  SendEmail = 9
}
