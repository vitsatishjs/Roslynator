
# `format` Command

Formats documents in the specified project or solution.

## Synopsis

```
roslynator format <PROJECT|SOLUTION>
[--culture]
[--end-of-line]
[--include-generated-code]
[--ignored-projects]
[--language]
[--file-log]
[--file-log-verbosity]
[--msbuild-path]
[--projects]
[-v|--verbosity]
```

## Arguments

**`PROJECT|SOLUTION`**

The project or solution to fix.

### Optional Options

**`--culture`** <CULTURE_ID>

Defines culture that should be used to display diagnostic message.

**`--end-of-line`** {lf|crlf}

Defines end of line character(s).

**`--include-generated-code`**

Indicates whether generated code should be formatted.

**`--ignored-projects`** <PROJECT_NAME>

Defines project names that should not be formatted.

**`--language`** `{cs[harp]|v[isual-]b[asic])}`

Defines project language.

**`--msbuild-path`** <MSBUILD_PATH>

Defines a path to MSBuild. This option must be specified if there are multiple locations of MSBuild (usually multiple installations of Visual Studio).

**`--projects`** <PROJECT_NAME>

Defines projects that should be formatted.

**`-p|--properties`** `<NAME=VALUE>`

Defines one or more MSBuild properties.

**`-v|--verbosity`** `{q[uiet]|m[inimal]|n[ormal]|d[etailed]|diag[nostic]}`

Defines the amount of information to display in the log.

## See Also

* [Roslynator Command-Line Interface](README.md)
