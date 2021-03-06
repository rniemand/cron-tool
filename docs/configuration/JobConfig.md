[Home](/README.md) / [Config](/docs/configuration/README.md) / JobConfig

# JobConfig
Main cron-job configuration file.

```json
{
  "id": "",
  "enabled": true,
  "name": "My awesome job",
  "steps": [],
  "variables": {},
  "schedule": {}
}
```

Details on each option is listed below.

| Property | Type | Required | Default | Notes |
| --- | --- | ---- | ---- | --- |
| `id` | `string` | required | - | Unique ID for the current job, used for scheduling. |
| `enabled` | `bool` | required | `true` | Enables the current job. |
| `name` | `string` | required | - | The name of the current job. |
| `steps` | [JobStepConfig](/docs/models/JobStepConfig.md)[] | required | `[]` | Array of [JobStepConfig](/docs/models/JobStepConfig.md) entries that make up this job. |
| `variables` | `Dictionary<string, object>` | optional | `{}` | Dictionary of variables accessable to all steps in this job. |
| `schedule` | [JobSchedule](/docs/configuration/JobSchedule.md) | optional | - | Defines the schedule (if any) to use when running the job. |

## Variables
Variables can be accessed by using the `${var:<name>}` syntax in any supported [Job Action](/docs/job-actions/README.md) property, these are case-sensitive.

**Note:** `String` values set as variables get additional processing when being initially set for a job.
