[Home](/README.md) / [Formatters](/docs/formatters/README.md) / DateTimeFormatter

# DateTimeFormatter
Replaces matching tags with a formatted `DateTime` value, supported tags need to follow the below convention.

```
{date:<format>}
```

## Supported Argument Types
The following [ArgTypes](/docs/enums/ArgType.md) are supported:

- `ArgType.File` - file paths
- `ArgType.Directory` - directory paths
- `ArgType.String` - general strings

## References
Additional reading on datetime formatting in `C#`.

- https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings