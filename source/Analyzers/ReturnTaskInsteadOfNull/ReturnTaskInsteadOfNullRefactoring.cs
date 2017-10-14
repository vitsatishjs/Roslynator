// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
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

            if (methodSymbol.MethodKind != MethodKind.Ordinary)
                return;

            if (!methodSymbol.ReturnType.IsConstructedFrom(taskOfTSymbol))
                return;

            var methodDeclaration = (MethodDeclarationSyntax)methodSymbol.GetSyntax(context.CancellationToken);

            ArrowExpressionClauseSyntax expressionBody = methodDeclaration.ExpressionBody;

            if (expressionBody != null)
            {
                AnalyzeExpressionBody(context, expressionBody.Expression);
            }
            else
            {
                AnalyzeBlock(context, methodDeclaration.Body);
            }
        }

        public static void AnalyzeLocalFunction(SyntaxNodeAnalysisContext context, INamedTypeSymbol taskOfTSymbol)
        {
            var localFunction = (LocalFunctionStatementSyntax)context.Node;

            if (localFunction.Modifiers.Contains(SyntaxKind.AsyncKeyword))
                return;

            if (localFunction.ReturnType?.Kind() != SyntaxKind.GenericName)
                return;

            ArrowExpressionClauseSyntax expressionBody = localFunction.ExpressionBody;

            if (expressionBody != null)
            {
                ExpressionSyntax expression = expressionBody.Expression;

                if (expression?.WalkDownParentheses().IsKind(SyntaxKind.NullLiteralExpression, SyntaxKind.DefaultExpression) != true)
                    return;

                if (!IsReturnTypeConstructedFromTaskOfT(localFunction, taskOfTSymbol, context.SemanticModel, context.CancellationToken))
                    return;

                context.ReportDiagnostic(DiagnosticDescriptors.ReturnTaskInsteadOfNull, expression);
            }
            else
            {
                BlockSyntax body = localFunction.Body;

                if (body == null)
                    return;

                if (!IsReturnTypeConstructedFromTaskOfT(localFunction, taskOfTSymbol, context.SemanticModel, context.CancellationToken))
                    return;

                AnalyzeBlock(context, body);
            }
        }

        public static void AnalyzeLambdaExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol taskOfTSymbol)
        {
            var lambda = (LambdaExpressionSyntax)context.Node;

            if (lambda.AsyncKeyword.Kind() == SyntaxKind.AsyncKeyword)
                return;

            CSharpSyntaxNode body = lambda.Body;

            if (body is ExpressionSyntax expression)
            {
                Analyze(context, lambda, expression, taskOfTSymbol);
            }
            else if (body is BlockSyntax block)
            {
                if (!IsReturnTypeConstructedFromTaskOfT(lambda, taskOfTSymbol, context.SemanticModel, context.CancellationToken))
                    return;

                AnalyzeBlock(context, block);
            }
        }

        public static void AnalyzeAnonymousMethod(SyntaxNodeAnalysisContext context, INamedTypeSymbol taskOfTSymbol)
        {
            var anonymousMethod = (AnonymousMethodExpressionSyntax)context.Node;

            if (anonymousMethod.AsyncKeyword.Kind() == SyntaxKind.AsyncKeyword)
                return;

            if (!IsReturnTypeConstructedFromTaskOfT(anonymousMethod, taskOfTSymbol, context.SemanticModel, context.CancellationToken))
                return;

            AnalyzeBlock(context, anonymousMethod.Block);
        }

        private static void Analyze(
            SyntaxNodeAnalysisContext context,
            SyntaxNode node,
            ExpressionSyntax expression,
            INamedTypeSymbol taskOfTSymbol)
        {
            if (expression?.WalkDownParentheses().IsKind(SyntaxKind.NullLiteralExpression, SyntaxKind.DefaultExpression) != true)
                return;

            if (!IsReturnTypeConstructedFromTaskOfT(node, taskOfTSymbol, context.SemanticModel, context.CancellationToken))
                return;

            context.ReportDiagnostic(DiagnosticDescriptors.ReturnTaskInsteadOfNull, expression);
        }

        private static bool IsReturnTypeConstructedFromTaskOfT(LocalFunctionStatementSyntax localFunction, INamedTypeSymbol taskOfTSymbol, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            var methodSymbol = semanticModel.GetDeclaredSymbol(localFunction, cancellationToken) as IMethodSymbol;

            return methodSymbol?.IsErrorType() == false
                && methodSymbol.ReturnType.IsConstructedFrom(taskOfTSymbol);
        }

        private static bool IsReturnTypeConstructedFromTaskOfT(SyntaxNode node, INamedTypeSymbol taskOfTSymbol, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            var methodSymbol = semanticModel.GetSymbol(node, cancellationToken) as IMethodSymbol;

            return methodSymbol?.IsErrorType() == false
                && methodSymbol.ReturnType.IsConstructedFrom(taskOfTSymbol);
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

        private static void AnalyzeBlock(SymbolAnalysisContext context, BlockSyntax body)
        {
            foreach (ExpressionSyntax expression in GetFixableExpressions(body))
                context.ReportDiagnostic(DiagnosticDescriptors.ReturnTaskInsteadOfNull, expression);
        }

        private static void AnalyzeBlock(SyntaxNodeAnalysisContext context, BlockSyntax body)
        {
            foreach (ExpressionSyntax expression in GetFixableExpressions(body))
                context.ReportDiagnostic(DiagnosticDescriptors.ReturnTaskInsteadOfNull, expression);
        }

        private static ImmutableArray<ExpressionSyntax> GetFixableExpressions(BlockSyntax body)
        {
            if (body == null)
                return ImmutableArray<ExpressionSyntax>.Empty;

            ReturnTaskInsteadOfNullWalker walker = ReturnTaskInsteadOfNullWalkerCache.Acquire();

            walker.VisitBlock(body);

            return ReturnTaskInsteadOfNullWalkerCache.GetExpressionsAndRelease(walker);
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

        public static InvocationExpressionSyntax CreateNewExpression(
            ExpressionSyntax expression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            var typeSymbol = semanticModel.GetTypeInfo(expression, cancellationToken).ConvertedType as INamedTypeSymbol;

            int position = expression.SpanStart;

            TypeSyntax type = typeSymbol.ToMinimalTypeSyntax(semanticModel, position);

            InvocationExpressionSyntax newNode = SimpleMemberInvocationExpression(
                semanticModel.GetTypeByMetadataName(MetadataNames.System_Threading_Tasks_Task).ToMinimalTypeSyntax(semanticModel, position),
                GenericName("FromResult", typeSymbol.TypeArguments[0].ToMinimalTypeSyntax(semanticModel, position)),
                Argument(typeSymbol.ToDefaultValueSyntax(type)));

            return newNode.WithTriviaFrom(expression);
        }
    }
}
