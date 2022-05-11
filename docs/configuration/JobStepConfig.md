[Home](/README.md) / [Config](/docs/configuration/README.md) / JobStepConfig

# JobStepConfig
Cron job step configuration, please refer to the [available actions page](/docs/job-actions/README.md) for a complete list of `JobActions` and their expected configuration.

```json
{
  "enabled": true,
  "name": "Step 1: Do something",
  "action": "CopyFile",
  "args": {}
}
```

Details on each option is listed below.

| Property | Type | Required | Default | Notes |
| --- | --- | ---- | ---- | --- |
