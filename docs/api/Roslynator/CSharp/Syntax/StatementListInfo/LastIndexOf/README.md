# StatementListInfo\.LastIndexOf Method

[Home](../../../../../README.md)

**Containing Type**: [StatementListInfo](../README.md)

**Assembly**: Roslynator\.CSharp\.dll

## Overloads

| Method | Summary |
| ------ | ------- |
| [LastIndexOf(Func\<StatementSyntax, Boolean>)](#Roslynator_CSharp_Syntax_StatementListInfo_LastIndexOf_System_Func_Microsoft_CodeAnalysis_CSharp_Syntax_StatementSyntax_System_Boolean__) | Searches for a statement that matches the predicate and returns returns zero\-based index of the last occurrence in the list\. |
| [LastIndexOf(StatementSyntax)](#Roslynator_CSharp_Syntax_StatementListInfo_LastIndexOf_Microsoft_CodeAnalysis_CSharp_Syntax_StatementSyntax_) | Searches for a statement and returns zero\-based index of the last occurrence in the list\. |

## LastIndexOf\(Func\<StatementSyntax, Boolean>\) <a id="Roslynator_CSharp_Syntax_StatementListInfo_LastIndexOf_System_Func_Microsoft_CodeAnalysis_CSharp_Syntax_StatementSyntax_System_Boolean__"></a>

\
Searches for a statement that matches the predicate and returns returns zero\-based index of the last occurrence in the list\.

```csharp
public int LastIndexOf(Func<Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax, bool> predicate)
```

### Parameters

**predicate** &ensp; [Func](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)\<[StatementSyntax](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.statementsyntax), [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)>

### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)

## LastIndexOf\(StatementSyntax\) <a id="Roslynator_CSharp_Syntax_StatementListInfo_LastIndexOf_Microsoft_CodeAnalysis_CSharp_Syntax_StatementSyntax_"></a>

\
Searches for a statement and returns zero\-based index of the last occurrence in the list\.

```csharp
public int LastIndexOf(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax statement)
```

### Parameters

**statement** &ensp; [StatementSyntax](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntax.statementsyntax)

### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)

