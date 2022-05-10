[Home](/README.md) / [Config](/docs/configuration/README.md) / JobConfig

# JobConfig
Main cron-job configuration file.

```json
{
  "enabled": true,
  "name": "My awesome job",
  "steps": []
}
```

Details on each option is listed below.

| Property | Type | Required | Default | Notes |
| --- | --- | ---- | ---- | --- |
| `enabled` | `bool` | required | `true` | Enables the current job. |
| `name` | `string` | required | - | The name of the current job. |
| `steps` | [JobStepConfig](/docs/models/JobStepConfig.md)[] | 