[Home](/README.md) / [Docs](/docs/README.md) / [Examples](/docs/examples/README.md) / BackupNasLandingPage

# BackupNasLandingPage
Holder.

```json
{
  "name": "Backup 'nas-landing-page' data (time-stamped)",
  "steps": [
    {
      "name": "Zip Satisfactory game saves",
      "action": "ZipFolder",
      "args": {
        "SourceDir": "\\\\192.168.0.60\\appdata\\nas-landing-page",
        "TargetZip": "\\\\192.168.0.60\\Backups\\nas-landing-page\\nas-landing-page-{date:yyyyMMdd-HHmm}.zip",
        "QuickZip": false,
        "IncludeBaseDir": true,
        "DeleteIfExists": true
      }
    }
  ]
}
```
