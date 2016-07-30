﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pihrtsoft.CodeAnalysis.CSharp.Refactoring
{
    internal static class BinaryExpressionRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, BinaryExpressionSyntax binaryExpression)
        {
            FormatBinaryExpressionRefactoring.ComputeRefactorings(context, binaryExpression);

            if (binaryExpression.IsKind(SyntaxKind.LogicalAndExpression, SyntaxKind.LogicalOrExpression)
                && binaryExpression.Left?.IsMissing == false
                && binaryExpression.Right?.IsMissing == false)
            {
                if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.AddBooleanComparison))
                {
                    if (binaryExpression.Left.Span.Contains(context.Span))
                    {
                        await AddBooleanComparisonRefactoring.ComputeRefactoringAsync(context, binaryExpression.Left).ConfigureAwait(false);
                    }
                    else if (binaryExpression.Right.Span.Contains(context.Span))
                    {
                        await AddBooleanComparisonRefactoring.ComputeRefactoringAsync(context, binaryExpression.Right).ConfigureAwait(false);
                    }
                }

                if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.NegateBinaryExpression))
                {
                    context.RegisterRefactoring(
                        "Negate binary expression",
                        cancellationToken =>
                        {
                            return NegateBinaryExpressionRefactoring.RefactorAsync(
                                context.Document,
                                binaryExpression,
                                cancellationToken);
                        });
                }
            }

            if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.ExpandCoalesceExpression)
                && binaryExpression.OperatorToken.Span.Contains(context.Span)
                && ExpandCoalesceExpressionRefactoring.CanRefactor(binaryExpression))
            {
                context.RegisterRefactoring(
                    "Expand coalesce expression",
                    cancellationToken =>
                    {
                        return ExpandCoalesceExpressionRefactoring.RefactorAsync(
                            context.Document,
                            binaryExpression,
                            cancellationToken);
                    });
            }

            if (context.Settings.IsAnyRefactoringEnabled(
                    RefactoringIdentifiers.MergeStringLiterals,
                    RefactoringIdentifiers.MergeStringLiteralsIntoMultilineStringLiteral)
                && MergeStringLiteralsRefactoring.CanRefactor(context, binaryExpression))
            {
                if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.MergeStringLiterals))
                {
                    context.RegisterRefactoring(
                        "Merge string literals",
                        cancellationToken => MergeStringLiteralsRefactoring.RefactorAsync(context.Document, binaryExpression, cancellationToken: cancellationToken));
                }

                if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.MergeStringLiteralsIntoMultilineStringLiteral)
                    && binaryExpression
                        .DescendantTrivia(binaryExpression.Span)
                        .Any(f => f.IsKind(SyntaxKind.EndOfLineTrivia)))
                {
                    context.RegisterRefactoring(
                        "Merge string literals into multiline string literal",
                        cancellationToken => MergeStringLiteralsRefactoring.RefactorAsync(context.Document, binaryExpression, multiline: true, cancellationToken: cancellationToken));
                }
            }

            if (context.Settings.IsRefactoringEnabled(RefactoringIdentifiers.SwapExpressionsInBinaryExpression)
                && binaryExpression.Left?.IsMissing == false
                && binaryExpression.Right?.IsMissing == false
                && binaryExpression.IsKind(SyntaxKind.LogicalAndExpression, SyntaxKind.LogicalOrExpression)
                && binaryExpression.OperatorToken.Span.Contains(context.Span))
            {
                context.RegisterRefactoring(
                    "Swap expressions",
                    cancellationToken =>
                    {
                        return SwapExpressionsRefactoring.RefactorAsync(
                            context.Document,
                            binaryExpression,
                            cancellationToken);
                    });
            }
        }
    }
}
