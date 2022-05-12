[Home](/README.md) / [Config](/docs/configuration/README.md) / JobStepCondition

# JobStepCondition
Represents a collection of `conditions` along with a [ConditionType](/docs/enums/ConditionType.md) used to join the provided conditions.

```json
{
  "type": "and",
  "expressions": [
    "backupFile.fileExists = true",
    "backupFile.fileSize > 100"
  ]
}
```

Details on each option is listed below.

| Property | Type | Required | Default | Notes |
| --- | --- | ---- | ---- | --- |
| `type` | [ConditionType](/docs/enums/ConditionType.md) | optional | `And` | The joining logic to use for the provided conditions. |
| `expressions` | `string[]` | required | `[]` | Collection of expression(s) that need to be evaluated. |
