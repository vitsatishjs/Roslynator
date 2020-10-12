﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class WrapCallChainRefactoring
    {
        public static Task ComputeRefactoringsAsync(RefactoringContext context, ConditionalAccessExpressionSyntax conditionalAccessExpression)
        {
            return ComputeRefactoringsAsync(context, (ExpressionSyntax)conditionalAccessExpression);
        }

        public static Task ComputeRefactoringsAsync(RefactoringContext context, MemberAccessExpressionSyntax memberAccessExpression)
        {
            return ComputeRefactoringsAsync(context, (ExpressionSyntax)memberAccessExpression);
        }

        private static async Task ComputeRefactoringsAsync(RefactoringContext context, ExpressionSyntax expression)
        {
            if (!context.Span.IsEmptyAndContainedInSpan(expression))
                return;

            expression = CSharpUtility.GetTopmostExpressionInCallChain(expression);

            SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

            if (!IsFormattable(expression, semanticModel, context.CancellationToken))
                return;

            if (expression.IsSingleLine(includeExteriorTrivia: false))
            {
                context.RegisterRefactoring(
                    "Wrap call chain",
                    ct => SyntaxFormatter.WrapCallChainAsync(context.Document, expression, semanticModel, ct),
                    RefactoringIdentifiers.WrapCallChain);
            }
            else if (expression.DescendantTrivia(expression.Span).All(f => f.IsWhitespaceOrEndOfLineTrivia()))
            {
                context.RegisterRefactoring(
                    "Unwrap call chain",
                    ct => SyntaxFormatter.UnwrapExpressionAsync(context.Document, expression, ct),
                    RefactoringIdentifiers.WrapCallChain);
            }
        }

        private static bool IsFormattable(
            ExpressionSyntax expression,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            int levels = 0;

            foreach (SyntaxNode node in new MethodChain(expression))
            {
                switch (node.Kind())
                {
                    case SyntaxKind.SimpleMemberAccessExpression:
                        {
                            var memberAccess = (MemberAccessExpressionSyntax)node;

                            if (memberAccess.Expression.IsKind(SyntaxKind.ThisExpression))
                                return levels == 2;

                            if (semanticModel
                                .GetSymbol(memberAccess.Expression, cancellationToken)?
                                .Kind == SymbolKind.Namespace)
                            {
                                return levels == 2;
                            }

                            if (++levels == 2)
                                return true;

                            break;
                        }
                    case SyntaxKind.MemberBindingExpression:
                        {
                            if (++levels == 2)
                                return true;

                            break;
                        }
                }
            }

            return false;
        }
    }
}
