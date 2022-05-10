[Home](/README.md) / [Models](/docs/models/README.md) / JobConfig

# JobConfig
Main cron-job configuration file.

```json
{
  "enabled": true,
  "name": "My awesome job",
  "steps": []
}
```

## Step
Cron job step configuration, please refer to the [available actions page](/docs/job-actions/README.md) for a complete list of `JobActions` and their expected configuration.

```json
{
  "enabled": true,
  "name": "Step 1: Do something",
  "action": "CopyFile",
  "args": {}
}
```