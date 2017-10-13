// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.ReturnTaskInsteadOfNull
{
    internal static class ReturnTaskInsteadOfNullRefactoring
    {
        public static void AnalyzeMethodSymbol(SymbolAnalysisContext context, INamedTypeSymbol taskOfTSymbol)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            if (methodSymbol.ReturnsVoid)
                return;

            if (methodSymbol.IsAsync)
                return;

            if (methodSymbol.IsImplicitlyDeclared)
                return;

            if (!methodSymbol.MethodKind.Is(
                MethodKind.Ordinary,
                MethodKind.AnonymousFunction,
                MethodKind.LocalFunction))
            {
                return;
            }

            if (!methodSymbol.ReturnType.IsConstructedFrom(taskOfTSymbol))
                return;

            SyntaxNode node = methodSymbol.GetSyntax(context.CancellationToken);

            switch (node.Kind())
            {
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)node;

                        ArrowExpressionClauseSyntax expressionBody = methodDeclaration.ExpressionBody;

                        if (expressionBody != null)
                        {
                            AnalyzeExpressionBody(context, expressionBody.Expression);
                        }
                        else
                        {
                            AnalyzeBlock(context, methodDeclaration.Body);
                        }

                        break;
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        var localFunction = (LocalFunctionStatementSyntax)node;

                        ArrowExpressionClauseSyntax expressionBody = localFunction.ExpressionBody;

                        if (expressionBody != null)
                        {
                            AnalyzeExpressionBody(context, expressionBody.Expression);
                        }
                        else
                        {
                            AnalyzeBlock(context, localFunction.Body);
                        }

                        break;
                    }
                case SyntaxKind.SimpleLambdaExpression:
                case SyntaxKind.ParenthesizedLambdaExpression:
                    {
                        var lambda = (LambdaExpressionSyntax)node;

                        CSharpSyntaxNode body = lambda.Body;

                        if (body is ExpressionSyntax expression)
                        {
                            AnalyzeExpressionBody(context, expression);
                        }
                        else if (body is BlockSyntax block)
                        {
                            AnalyzeBlock(context, block);
                        }

                        break;
                    }
                case SyntaxKind.AnonymousMethodExpression:
                    {
                        var anonymousMethod = (AnonymousMethodExpressionSyntax)node;

                        AnalyzeBlock(context, anonymousMethod.Block);

                        break;
                    }
                default:
                    {
                        Debug.Fail(node.Kind().ToString());
                        break;
                    }
            }
        }

        public static void AnalyzePropertySymbol(SymbolAnalysisContext context, INamedTypeSymbol taskOfTSymbol)
        {
            var propertySymbol = (IPropertySymbol)context.Symbol;

            if (propertySymbol.IsImplicitlyDeclared)
                return;

            if (propertySymbol.GetMethod == null)
                return;

            if (!propertySymbol.Type.IsConstructedFrom(taskOfTSymbol))
                return;

            SyntaxNode node = propertySymbol.GetSyntax(context.CancellationToken);

            switch (node.Kind())
            {
                case SyntaxKind.PropertyDeclaration:
                    {
                        var property = (PropertyDeclarationSyntax)node;

                        ArrowExpressionClauseSyntax expressionBody = property.ExpressionBody;

                        if (expressionBody != null)
                        {
                            AnalyzeExpressionBody(context, expressionBody.Expression);
                        }
                        else
                        {
                            AnalyzeGetAccessor(context, property.Getter());
                        }

                        break;
                    }
                case SyntaxKind.IndexerDeclaration:
                    {
                        var indexer = (IndexerDeclarationSyntax)node;

                        ArrowExpressionClauseSyntax expressionBody = indexer.ExpressionBody;

                        if (expressionBody != null)
                        {
                            AnalyzeExpressionBody(context, expressionBody.Expression);
                        }
                        else
                        {
                            AnalyzeGetAccessor(context, indexer.Getter());
                        }

                        break;
                    }
                default:
                    {
                        Debug.Fail(node.Kind().ToString());
                        break;
                    }
            }
        }

        private static void AnalyzeBlock(SymbolAnalysisContext context, BlockSyntax body)
        {
            if (body == null)
                return;

            ReturnTaskInsteadOfNullWalker walker = ReturnTaskInsteadOfNullWalkerCache.Acquire();

            walker.VisitBlock(body);

            ImmutableArray<ExpressionSyntax> expressions = ReturnTaskInsteadOfNullWalkerCache.GetExpressionsAndRelease(walker);

            if (!expressions.Any())
                return;

            foreach (ExpressionSyntax expression in expressions)
                context.ReportDiagnostic(DiagnosticDescriptors.ReturnTaskInsteadOfNull, expression);
        }

        private static void AnalyzeExpressionBody(SymbolAnalysisContext context, ExpressionSyntax expression)
        {
            if (expression?
                .WalkDownParentheses()
                .IsKind(SyntaxKind.NullLiteralExpression, SyntaxKind.DefaultExpression) == true)
            {
                context.ReportDiagnostic(DiagnosticDescriptors.ReturnTaskInsteadOfNull, expression);
            }
        }

        private static void AnalyzeGetAccessor(SymbolAnalysisContext context, AccessorDeclarationSyntax getter)
        {
            if (getter == null)
                return;

            ArrowExpressionClauseSyntax expressionBody = getter.ExpressionBody;

            if (expressionBody != null)
            {
                AnalyzeExpressionBody(context, expressionBody.Expression);
            }
            else
            {
                AnalyzeBlock(context, getter.Body);
            }
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            ReturnStatementSyntax returnStatement,
            CancellationToken cancellationToken)
        {
            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            ExpressionSyntax expression = returnStatement.Expression;

            ITypeSymbol typeSymbol = semanticModel.GetTypeInfo(expression, cancellationToken).ConvertedType;

            TypeSyntax type = typeSymbol.ToMinimalTypeSyntax(semanticModel, expression.SpanStart);

            InvocationExpressionSyntax newNode = SimpleMemberInvocationExpression(
                IdentifierName("Task"),
                GenericName("FromResult", type),
                Argument(typeSymbol.ToDefaultValueSyntax(type)));

            newNode = newNode.WithTriviaFrom(expression);

            return await document.ReplaceNodeAsync(expression, newNode, cancellationToken).ConfigureAwait(false);
        }
    }
}
