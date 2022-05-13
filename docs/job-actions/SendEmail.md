[Home](/README.md) / [Job Actions](/docs/job-actions/README.md) / Send Email

# Send Email
Provided by the `SendEmailAction` class, enables the sending of emails from your jobs.

```json
{
  "enabled": true,
  "name": "Send an email",
  "action": "SendEmail",
  "args": {
    "ToAddress": "email@address.com",
    "ToName": "Recipient Name",
    "Subject": "Mail Title",
    "Body": "This is the body of your email"
  }
}
```

## Supported Arguments
Below is a breakdown of each argument, please refer to the [ArgType](/docs/enums/ArgType.md) documentation for a list of all supported argument types.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `ToAddress` | `Email` | required | - | The email address to send the message to. |
| `ToName` | `string` | optional | - | The recipients name - if not supplied will default to the `ToAddress`. |
| `Subject` | `string` | required | - | The subject field for the sent mail. |
| `Body` | `string` | required | - | The body of the email. |

## Required Global Arguments
The following global arguments are required for this job step to work.

| Property | Type | Required | Default | Notes |
| --- | --- | --- | --- | --- |
| `mail.host` | `string` | optional | smtp.gmail.com | Your email host. |
| `mail.port` | `int` | optional | 587 | Your email host port. |
| `mail.username` | `string` | required | - | Login name for your mail provider. |
| `mail.password` | `string` | required | - | Password for your mail client. |
| `mail.fromAddress` | `EMail` | required | - | The from address to use when sending an email. |
| `mail.fromName` | `string` | required | - | From name to use when sending emails. |

Please refer to [this page](/docs/general/ConfiguringMail.md) for information on setting up GMail.