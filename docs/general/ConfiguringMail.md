[Home](/README.md) / [General](/docs/general/README.md) / Configuring Mail

# Configuring Mail
This guid will run you through the basics of configuring GMail to work with the `cron-tool`.

This guide is intended to assist users who have `MFA` (Multi  Factor Authentication) enabled on their accounts.

- Open a browser and head over to [Google -> Settings -> Security -> Passwords](https://security.google.com/settings/security/apppasswords)
- Use the wizard to create a new `Mail` password for your account
- For a device select `Other` from the drop-down menu and give it a meaningful name
- Take note of the auto-generated password as you will need it later on
- In the root directory where `cron-tool` loads it's configuration from (defined in your [appsettings.json](/docs/configuration/appsettings.md) file) create a file called `globals.json`
- Add the following configuration to the file:

```json
{
  "mail.host": "smtp.gmail.com",
  "mail.port": 587,
  "mail.username": "your-email",
  "mail.password": "google-password-from-above",
  "mail.fromAddress": "desired-from-address",
  "mail.fromName": "desired-from-name"
}
```

That should cover the basic configuration needed to use the [SendEmail](/docs/job-actions/SendEmail.md) job action.
