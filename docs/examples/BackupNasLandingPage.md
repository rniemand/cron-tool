[Home](/README.md) / [Examples](/docs/examples/README.md) / BackupNasLandingPage

# BackupNasLandingPage
The example below will backup the `\\192.168.0.60\appdata\nas-landing-page` directory and all of it's content to a zip file with a dynamic name `\\192.168.0.60\Backups\nas-landing-page\nas-landing-page-{date:yyyyMMdd}.zip`.

```json
{
  "name": "Backup 'nas-landing-page' data (time-stamped)",
  "id": "3B5F5A62-DEA4-4C2A-B900-868DBD1C0E51",
  "steps": [
    {
      "name": "Zip Satisfactory game saves",
      "action": "ZipFolder",
      "args": {
        "SourceDir": "\\\\192.168.0.60\\appdata\\nas-landing-page",
        "TargetZip": "\\\\192.168.0.60\\Backups\\nas-landing-page\\nas-landing-page-{date:yyyyMMdd}.zip",
        "QuickZip": false,
        "IncludeBaseDir": true,
        "DeleteIfExists": true
      }
    }
  ]
}
```

Once run this should produce a `ZIP` file with a name similar to the follwing:

    \\192.168.0.60\Backups\nas-landing-page\nas-landing-page-20220506.zip
