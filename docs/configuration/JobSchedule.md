[Home](/README.md) / [Config](/docs/configuration/README.md) / JobSchedule

# JobSchedule
Used to define a running schedule for your [job](/docs/configuration/JobConfig.md) - this is an optional field.

```json
{
  "frequency": "Hour",
  "intValue": 12,
  "runOnStart": true
}
```

Details on each option is listed below.

| Property | Type | Required | Default | Notes |
| --- | --- | ---- | ---- | --- |
| `frequency` | [ScheduleFrequency](/docs/enums/ScheduleFrequency.md) | optional | `Hour` | Frequency modifier to use when scheduling job runs. |
| `intValue` | `int` | optional | `12` | Numeric value to use when scheduling the next job run. |
| `runOnStart` | `bool` | optional | `false` | When `true` this job will run on application start overwriting any previously set schedule value. |
