[Home](/README.md) / [Config](/docs/configuration/README.md) / JobSchedule

# JobSchedule
Used to define a running schedule for your [job](/docs/configuration/JobConfig.md) - this is an optional field.

```json
{
  "frequency": "TimeOfDay",
  "intValue": 0,
  "runOnStart": true,
  "timeOfDay": "08:05"
}
```

## Configuration
Details on each option is listed below.

| Property | Type | Required | Default | Notes |
| --- | --- | ---- | ---- | --- |
| `frequency` | [ScheduleFrequency](/docs/enums/ScheduleFrequency.md) | optional | `Hour` | Frequency modifier to use when scheduling job runs. |
| `intValue` | `int` | optional | `12` | Numeric value to use when scheduling the next job run. |
| `runOnStart` | `bool` | optional | `false` | When `true` this job will run on application start overwriting any previously set schedule value. |
| `timeOfDay` | `string` | optional | - | Used with the `TimeOfDay` [ScheduleFrequency](/docs/enums/ScheduleFrequency.md) to express the time of day, e.g. `08:30` |

## Notes
Additional notes for specific configuration options.

### timeOfDay
When using the `TimeOfDay` [ScheduleFrequency](/docs/enums/ScheduleFrequency.md) be sure to use the `hh:MM` format when setting the value, failing to use this format will result in an exception being thrown, below are some exmaples:

- `16:45` - runs every day at 4:45 PM
- `08:05` - runs every day at 8:05 AM
