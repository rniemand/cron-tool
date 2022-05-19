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
    "Body": "This is the body of your email",
    "Template": "mail-template"
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
| `Template` | `string` | optional | `default` | The name of the mail template to use. |

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
| `mail.deliveryFormat` | [SmtpDeliveryFormat](https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.smtpdeliveryformat?view=net-6.0) | optional | `SevenBit` | Delivery format to use. |
| `mail.deliveryMethod` | [SmtpDeliveryMethod](https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.smtpdeliverymethod?view=net-6.0) | optional | `Network` | Delivery method to use when sending emails. |
| `mail.enableSsl` | `bool` | optional | `true` | Enables the usage of SSL. |
| `mail.timeout` | `int` | optional | `30000` | Timeout to use when sending emails. |
| `mail.encoding` | [Encoding](https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=net-6.0) | optional | `UTF8` | The encoding to use when sending emails. |
| `mail.templateDir` | `Path` | optional | `{root}mail-tpl` | Path to mail template directory. |

Please refer to [this page](/docs/general/ConfiguringMail.md) for information on setting up GMail.
