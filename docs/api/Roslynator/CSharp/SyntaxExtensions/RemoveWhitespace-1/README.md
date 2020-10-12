# SyntaxExtensions\.RemoveWhitespace\<TNode>\(TNode, TextSpan?\) Method

[Home](../../../../README.md)

**Containing Type**: [SyntaxExtensions](../README.md)

**Assembly**: Roslynator\.CSharp\.dll

\
Creates a new node with the whitespace removed\.

```csharp
public static TNode RemoveWhitespace<TNode>(this TNode node, Microsoft.CodeAnalysis.Text.TextSpan? span = null) where TNode : Microsoft.CodeAnalysis.SyntaxNode
```

### Type Parameters

**TNode**

### Parameters

**node** &ensp; TNode

**span** &ensp; [TextSpan](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.text.textspan)?

### Returns

TNode

