﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.Refactorings
{
    internal static class RemoveAsyncAwait
    {
        public static async Task<Document> RefactorAsync(
            Document document,
            SyntaxToken asyncKeyword,
            CancellationToken cancellationToken = default)
        {
            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            SyntaxNode node = asyncKeyword.Parent;

            var remover = new AwaitRemover(semanticModel, cancellationToken);

            SyntaxNode newNode = GetNewNode();

            return await document.ReplaceNodeAsync(node, newNode, cancellationToken).ConfigureAwait(false);

            SyntaxNode GetNewNode()
            {
                switch (node)
                {
                    case MethodDeclarationSyntax methodDeclaration:
                        {
                            return remover
                                .VisitMethodDeclaration(methodDeclaration)
                                .RemoveModifier(SyntaxKind.AsyncKeyword);
                        }
                    case LocalFunctionStatementSyntax localFunction:
                        {
                            BlockSyntax body = localFunction.Body;

                            if (body != null)
                            {
                                localFunction = localFunction.WithBody((BlockSyntax)remover.VisitBlock(body));
                            }
                            else
                            {
                                ArrowExpressionClauseSyntax expressionBody = localFunction.ExpressionBody;

                                if (expressionBody != null)
                                    localFunction = localFunction.WithExpressionBody((ArrowExpressionClauseSyntax)remover.VisitArrowExpressionClause(expressionBody));
                            }

                            return localFunction.RemoveModifier(SyntaxKind.AsyncKeyword);
                        }
                    case SimpleLambdaExpressionSyntax lambda:
                        {
                            return lambda
                                .WithBody((CSharpSyntaxNode)remover.Visit(lambda.Body))
                                .WithAsyncKeyword(GetMissingAsyncKeyword(lambda.AsyncKeyword));
                        }
                    case ParenthesizedLambdaExpressionSyntax lambda:
                        {
                            return lambda
                                .WithBody((CSharpSyntaxNode)remover.Visit(lambda.Body))
                                .WithAsyncKeyword(GetMissingAsyncKeyword(lambda.AsyncKeyword));
                        }
                    case AnonymousMethodExpressionSyntax anonymousMethod:
                        {
                            return anonymousMethod
                                .WithBody((CSharpSyntaxNode)remover.Visit(anonymousMethod.Body))
                                .WithAsyncKeyword(GetMissingAsyncKeyword(anonymousMethod.AsyncKeyword));
                        }
                    default:
                        {
                            throw new InvalidOperationException();
                        }
                }
            }
        }

        private static SyntaxToken GetMissingAsyncKeyword(SyntaxToken asyncKeyword)
        {
            if (asyncKeyword.TrailingTrivia.All(f => f.IsWhitespaceTrivia()))
            {
                return MissingToken(SyntaxKind.AsyncKeyword).WithLeadingTrivia(asyncKeyword.LeadingTrivia);
            }
            else
            {
                return MissingToken(SyntaxKind.AsyncKeyword).WithTriviaFrom(asyncKeyword);
            }
        }

        private class AwaitRemover : CSharpSyntaxRewriter
        {
            public AwaitRemover(SemanticModel semanticModel, CancellationToken cancellationToken)
            {
                SemanticModel = semanticModel;
                CancellationToken = cancellationToken;
            }

            public SemanticModel SemanticModel { get; }

            public CancellationToken CancellationToken { get; }

            private static ExpressionSyntax ExtractExpressionFromAwait(AwaitExpressionSyntax awaitExpression, SemanticModel semanticModel, CancellationToken cancellationToken)
            {
                ExpressionSyntax expression = awaitExpression.Expression;

                if (semanticModel.GetTypeSymbol(expression, cancellationToken) is INamedTypeSymbol typeSymbol)
                {
                    if (typeSymbol.HasMetadataName(MetadataNames.System_Runtime_CompilerServices_ConfiguredTaskAwaitable)
                        || typeSymbol.OriginalDefinition.HasMetadataName(MetadataNames.System_Runtime_CompilerServices_ConfiguredTaskAwaitable_T))
                    {
                        if (expression is InvocationExpressionSyntax invocation)
                        {
                            var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;

                            if (string.Equals(memberAccess?.Name?.Identifier.ValueText, "ConfigureAwait", StringComparison.Ordinal))
                                expression = memberAccess.Expression;
                        }
                    }
                }

                return expression.WithTriviaFrom(awaitExpression);
            }

            public override SyntaxNode VisitAwaitExpression(AwaitExpressionSyntax node)
            {
                node = (AwaitExpressionSyntax)base.VisitAwaitExpression(node);

                return ExtractExpressionFromAwait(node, SemanticModel, CancellationToken);
            }

            public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
            {
                return node;
            }

            public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
            {
                return node;
            }

            public override SyntaxNode VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
            {
                return node;
            }

            public override SyntaxNode VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
            {
                return node;
            }
        }
    }
}