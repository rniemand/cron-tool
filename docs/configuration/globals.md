[Home](/README.md) / [Config](/docs/configuration/README.md) / Globals

# Globals
If you create a `globals.json` file in the jobs config directory defined under your [appsettings.json](/docs/configuration/appsettings.md) file it will be loaded, parsed and added to all running [job](/docs/configuration/JobConfig.md) instances.

The contents of this file will be processed as a generic `Dictionary<string, string>`, a smaple of this can be seen below.

```json
{
  "mail.username": "niemandr"
}
```

You can access `GLOBAL` variables in your job steps using the `${global:<property>}` syntax.

    ${global:mail.username}
