[Home](/README.md) / [General](/docs/general/README.md) / Job Argument Processing

# Job Argument Processing
This section covers the `order of precedence` used when processing job steps.

At the moment values are evaluated in the following order:

- `${global:xx}` - finds and replaces all `GLOBAL` variables.
- `${var:xxx}` - finds and replaces all `JOB` scoped variables.
- `${state:xxx}` - finds and replaces all `STATE` variables.
- Any applicable formatters
