﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Refactorings;

namespace Roslynator.CSharp.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExtractMemberToNewDocumentCodeFixProvider))]
    [Shared]
    public class ExtractMemberToNewDocumentCodeFixProvider : AbstractCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticIdentifiers.DeclareEachTypeInSeparateFile); }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindFirstAncestorOrSelf(root, context.Span, out MemberDeclarationSyntax memberDeclaration))
                return;

            string name = ExtractTypeDeclarationToNewDocumentRefactoring.GetIdentifier(memberDeclaration).ValueText;
            string title = ExtractTypeDeclarationToNewDocumentRefactoring.GetTitle(name);

            CodeAction codeAction = CodeAction.Create(
                title,
                cancellationToken => ExtractTypeDeclarationToNewDocumentRefactoring.RefactorAsync(context.Document, memberDeclaration, cancellationToken),
                GetEquivalenceKey(DiagnosticIdentifiers.DeclareEachTypeInSeparateFile));

            context.RegisterCodeFix(codeAction, context.Diagnostics);
        }
    }
}
