// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.CodeFixes;

namespace Roslynator.CSharp.Analyzers.UnusedMemberDeclaration
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UnusedMemberDeclarationCodeFixProvider))]
    [Shared]
    public class UnusedMemberDeclarationCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticIdentifiers.RemoveUnusedMemberDeclaration); }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindNode(root, context.Span, out SyntaxNode node, predicate: f => f.IsKind(
                SyntaxKind.DelegateDeclaration,
                SyntaxKind.EventDeclaration,
                SyntaxKind.VariableDeclarator,
                SyntaxKind.MethodDeclaration,
                SyntaxKind.PropertyDeclaration)))
            {
                return;
            }

            SyntaxToken identifier = GetIdentifier(node);

            CodeAction codeAction = CodeAction.Create(
                $"Remove '{identifier.ValueText}'",
                cancellationToken => UnusedMemberDeclarationRefactoring.RefactorAsync(context.Document, node, cancellationToken),
                GetEquivalenceKey(DiagnosticIdentifiers.RemoveUnusedMemberDeclaration));

            context.RegisterCodeFix(codeAction, context.Diagnostics[0]);
        }

        private static SyntaxToken GetIdentifier(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.DelegateDeclaration:
                    return ((DelegateDeclarationSyntax)node).Identifier;
                case SyntaxKind.EventDeclaration:
                    return ((EventDeclarationSyntax)node).Identifier;
                case SyntaxKind.VariableDeclarator:
                    return ((VariableDeclaratorSyntax)node).Identifier;
                case SyntaxKind.MethodDeclaration:
                    return ((MethodDeclarationSyntax)node).Identifier;
                case SyntaxKind.PropertyDeclaration:
                    return ((PropertyDeclarationSyntax)node).Identifier;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
