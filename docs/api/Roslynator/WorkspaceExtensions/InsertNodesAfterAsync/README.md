# WorkspaceExtensions\.InsertNodesAfterAsync\(Document, SyntaxNode, IEnumerable\<SyntaxNode>, CancellationToken\) Method

[Home](../../../README.md)

**Containing Type**: [WorkspaceExtensions](../README.md)

**Assembly**: Roslynator\.Workspaces\.Core\.dll

\
Creates a new document with new nodes inserted after the specified node\.

```csharp
public static System.Threading.Tasks.Task<Microsoft.CodeAnalysis.Document> InsertNodesAfterAsync(this Microsoft.CodeAnalysis.Document document, Microsoft.CodeAnalysis.SyntaxNode nodeInList, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.SyntaxNode> newNodes, System.Threading.CancellationToken cancellationToken = default)
```

### Parameters

**document** &ensp; [Document](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.document)

**nodeInList** &ensp; [SyntaxNode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxnode)

**newNodes** &ensp; [IEnumerable](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)\<[SyntaxNode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxnode)>

**cancellationToken** &ensp; [CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken)

### Returns

[Task](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1)\<[Document](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.document)>

