# NullCheckStyles Enum

[Home](../../../README.md) &#x2022; [Fields](#fields)

**Namespace**: [Roslynator.CSharp](../README.md)

**Assembly**: Roslynator\.CSharp\.dll

\
Specifies a null check\.

```csharp
[Flags]
public enum NullCheckStyles
```

### Inheritance

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) &#x2192; [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) &#x2192; [Enum](https://docs.microsoft.com/en-us/dotnet/api/system.enum) &#x2192; NullCheckStyles

### Attributes

* [FlagsAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute)

## Fields

| Name | Value | Combination of | Summary |
| ---- | ----- | -------------- | ------- |
| None | 0 | | No null check specified\. |
| EqualsToNull | 1 | | `x == null` |
| NotEqualsToNull | 2 | | `x != null` |
| ComparisonToNull | 3 | EqualsToNull \| NotEqualsToNull | Expression that uses equality/inequality operator\. |
| IsNull | 4 | | `x is null` |
| NotIsNull | 8 | | `!(x is null)` |
| IsPattern | 12 | IsNull \| NotIsNull | Expression that uses pattern syntax\. |
| NotHasValue | 16 | | `!x.HasValue` |
| CheckingNull | 21 | EqualsToNull \| IsNull \| NotHasValue | Expression that checks whether an expression is null\. |
| HasValue | 32 | | `x.HasValue` |
| CheckingNotNull | 42 | NotEqualsToNull \| NotIsNull \| HasValue | Expression that checks whether an expression is not null\. |
| HasValueProperty | 48 | NotHasValue \| HasValue | Expression that uses [Nullable\<T>.HasValue](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1.hasvalue) property\. |
| All | 63 | ComparisonToNull \| IsPattern \| HasValueProperty | All null check styles\. |

