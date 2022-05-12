[Home](/README.md) / [Config](/docs/configuration/README.md) / JobStepConfig

# JobStepConfig
Cron job step configuration, please refer to the [available actions page](/docs/job-actions/README.md) for a complete list of `JobActions` and their expected configuration.

```json
{
  "enabled": true,
  "name": "Step 1: Do something",
  "action": "CopyFile",
  "args": {},
  "id": "myJob",
  "condition": {},
  "quitOnFailure": true
}
```

Details on each option is listed below.

| Property | Type | Required | Default | Notes |
| --- | --- | ---- | ---- | --- |
| `enabled` | `bool` | optional | `true` | Enables the current job step. |
| `name` | `string` | required | - | Unique name for this job step. |
| `action` | [JobStepAction](/docs/enums/JobStepAction.md) | required | `Unknown` | Desired [job step action](/docs/job-actions/README.md) you wish to execute. |
| `args` | `Dictionary<string, object>` | optional | `{}` | Job step action specific arguments. |
| `id` | `string` | optional | - | Unique ID used by the current job step to publish state (if supported by the selected step). If none is supplied it will be generated. |
| `condition` | [JobStepCondition](/docs/configuration/JobStepCondition.md) | optional | `null` | Optional condition required for this step to run. |
| `quitOnFailure` | `bool` | optional | `true` | Indicates that the job needs to be stopped if this step fails. |