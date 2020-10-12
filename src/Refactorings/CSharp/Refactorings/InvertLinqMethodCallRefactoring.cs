﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class InvertLinqMethodCallRefactoring
    {
        public static void ComputeRefactoring(RefactoringContext context, InvocationExpressionSyntax invocation, SemanticModel semanticModel)
        {
            if (!ComputeRefactoring(context, invocation, semanticModel, "Any", "All"))
                ComputeRefactoring(context, invocation, semanticModel, "All", "Any");
        }

        private static bool ComputeRefactoring(
            RefactoringContext context,
            InvocationExpressionSyntax invocation,
            SemanticModel semanticModel,
            string fromMethodName,
            string toMethodName)
        {
            IMethodSymbol methodSymbol = semanticModel.GetExtensionMethodInfo(invocation, context.CancellationToken).Symbol;

            if (methodSymbol == null)
                return false;

            if (!SymbolUtility.IsLinqExtensionOfIEnumerableOfTWithPredicate(methodSymbol, fromMethodName))
                return false;

            ExpressionSyntax expression = GetExpression(invocation);

            if (expression == null)
                return false;

            context.RegisterRefactoring(
                $"Invert '{fromMethodName}'",
                ct => RefactorAsync(context.Document, invocation, toMethodName, expression, ct),
                RefactoringIdentifiers.InvertLinqMethodCall);

            return true;
        }

        private static ExpressionSyntax GetExpression(InvocationExpressionSyntax invocation)
        {
            ExpressionSyntax expression = invocation
                .ArgumentList?
                .Arguments
                .Last()
                .Expression;

            switch (expression?.Kind())
            {
                case SyntaxKind.SimpleLambdaExpression:
                case SyntaxKind.ParenthesizedLambdaExpression:
                    {
                        return ((LambdaExpressionSyntax)expression).Body as ExpressionSyntax;
                    }
            }

            return null;
        }

        public static async Task<Document> RefactorAsync(
            Document document,
            InvocationExpressionSyntax invocationExpression,
            string memberName,
            ExpressionSyntax expression,
            CancellationToken cancellationToken = default)
        {
            var memberAccessExpression = (MemberAccessExpressionSyntax)invocationExpression.Expression;

            MemberAccessExpressionSyntax newMemberAccessExpression = memberAccessExpression
                .WithName(SyntaxFactory.IdentifierName(memberName).WithTriviaFrom(memberAccessExpression.Name));

            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            InvocationExpressionSyntax newNode = invocationExpression
                .ReplaceNode(expression, SyntaxInverter.LogicallyInvert(expression, semanticModel, cancellationToken))
                .WithExpression(newMemberAccessExpression);

            return await document.ReplaceNodeAsync(invocationExpression, newNode, cancellationToken).ConfigureAwait(false);
        }
    }
}
