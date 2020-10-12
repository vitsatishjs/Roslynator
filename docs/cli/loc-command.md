
# `loc` Command

Counts physical lines of code in the specified project or solution.

## Synopsis

```
roslynator loc <PROJECT|SOLUTION>
[--ignore-block-boundary]
[--ignored-projects]
[--include-comments]
[--include-generated-code]
[--include-preprocessor-directives]
[--include-whitespace]
[--language]
[--file-log]
[--file-log-verbosity]
[--msbuild-path]
[--projects]
[-p|--properties]
[-v|--verbosity]
```

## Arguments

**`PROJECT|SOLUTION`**

The project or solution to calculate lines of code in.

### Optional Options

**`--ignore-block-boundary`**

Indicates whether a line that contains only block boundary should not be counted.

In C# block boundary is opening brace or closing brace.

In Visual Basic block boundary is end-block-statement such as `End Class`.

**`--ignored-projects`** <PROJECT_NAME>

Defines project names that should not be fixed.

**`--include-comments`**

Indicates whether a line that contains only comment should be counted.

**`--include-generated-code`**

Indicates whether generated code should be counted.

**`--include-preprocessor-directives`**

Indicates whether preprocessor directive line should be counted.

**`--include-whitespace`**

Indicates whether white-space line should be counted.

**`--language`** `{cs[harp]|v[isual-]b[asic])}`

Defines project language.

**`--msbuild-path`** <MSBUILD_PATH>

Defines a path to MSBuild. This option must be specified if there are multiple locations of MSBuild (usually multiple installations of Visual Studio).

**`--projects`** <PROJECT_NAME>

Defines projects that should be analyzed.

**`-p|--properties`** `<NAME=VALUE>`

Defines one or more MSBuild properties.

**`-v|--verbosity`** `{q[uiet]|m[inimal]|n[ormal]|d[etailed]|diag[nostic]}`

Defines the amount of information to display in the log.

## See Also

* [Roslynator Command-Line Interface](README.md)
