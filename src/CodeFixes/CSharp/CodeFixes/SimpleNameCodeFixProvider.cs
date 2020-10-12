﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SimpleNameCodeFixProvider))]
    [Shared]
    public class SimpleNameCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                return ImmutableArray.Create(
                    CompilerDiagnosticIdentifiers.CannotConvertMethodGroupToNonDelegateType,
                    CompilerDiagnosticIdentifiers.TypeOrNamespaceNameCouldNotBeFound,
                    CompilerDiagnosticIdentifiers.NameIsNotValidInGivenContext);
            }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindFirstAncestorOrSelf(root, context.Span, out SimpleNameSyntax simpleName))
                return;

            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                switch (diagnostic.Id)
                {
                    case CompilerDiagnosticIdentifiers.CannotConvertMethodGroupToNonDelegateType:
                    case CompilerDiagnosticIdentifiers.NameIsNotValidInGivenContext:
                        {
                            if (!Settings.IsEnabled(diagnostic.Id, CodeFixIdentifiers.AddArgumentList))
                                break;

                            if (!simpleName.IsParentKind(SyntaxKind.SimpleMemberAccessExpression))
                                break;

                            var memberAccess = (MemberAccessExpressionSyntax)simpleName.Parent;

                            CodeAction codeAction = CodeAction.Create(
                                "Add argument list",
                                cancellationToken =>
                                {
                                    InvocationExpressionSyntax invocationExpression = InvocationExpression(
                                        memberAccess.WithoutTrailingTrivia(),
                                        ArgumentList().WithTrailingTrivia(memberAccess.GetTrailingTrivia()));

                                    return context.Document.ReplaceNodeAsync(memberAccess, invocationExpression, cancellationToken);
                                },
                                GetEquivalenceKey(diagnostic));

                            context.RegisterCodeFix(codeAction, diagnostic);
                            break;
                        }
                    case CompilerDiagnosticIdentifiers.TypeOrNamespaceNameCouldNotBeFound:
                        {
                            if (!Settings.IsEnabled(diagnostic.Id, CodeFixIdentifiers.ChangeArrayType))
                                break;

                            if (!(simpleName.Parent is ArrayTypeSyntax arrayType))
                                break;

                            if (!(arrayType.Parent is ArrayCreationExpressionSyntax arrayCreation))
                                break;

                            if (!object.ReferenceEquals(simpleName, arrayType.ElementType))
                                break;

                            ExpressionSyntax expression = arrayCreation.Initializer?.Expressions.FirstOrDefault();

                            if (expression == null)
                                break;

                            SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                            ITypeSymbol typeSymbol = semanticModel.GetTypeSymbol(expression, context.CancellationToken);

                            if (typeSymbol?.SupportsExplicitDeclaration() != true)
                                break;

                            TypeSyntax newType = typeSymbol.ToTypeSyntax()
                                .WithSimplifierAnnotation()
                                .WithTriviaFrom(simpleName);

                            CodeAction codeAction = CodeAction.Create(
                                $"Change element type to '{SymbolDisplay.ToMinimalDisplayString(typeSymbol, semanticModel, simpleName.SpanStart, SymbolDisplayFormats.DisplayName)}'",
                                cancellationToken => context.Document.ReplaceNodeAsync(simpleName, newType, cancellationToken),
                                GetEquivalenceKey(diagnostic));

                            context.RegisterCodeFix(codeAction, diagnostic);
                            break;
                        }
                }
            }
        }
    }
}
