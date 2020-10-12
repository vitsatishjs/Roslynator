﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class LiteralExpressionRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, LiteralExpressionSyntax literalExpression)
        {
            switch (literalExpression.Kind())
            {
                case SyntaxKind.TrueLiteralExpression:
                case SyntaxKind.FalseLiteralExpression:
                    {
                        if (context.IsRefactoringEnabled(RefactoringIdentifiers.InvertBooleanLiteral)
                            && literalExpression.Span.Contains(context.Span))
                        {
                            context.RegisterRefactoring(
                                "Invert boolean literal",
                                cancellationToken => InvertBooleanLiteralRefactoring.RefactorAsync(context.Document, literalExpression, cancellationToken),
                                RefactoringIdentifiers.InvertBooleanLiteral);
                        }

                        break;
                    }
                case SyntaxKind.StringLiteralExpression:
                    {
                        if (context.Span.IsContainedInSpanOrBetweenSpans(literalExpression))
                            await StringLiteralExpressionRefactoring.ComputeRefactoringsAsync(context, literalExpression).ConfigureAwait(false);

                        break;
                    }
                case SyntaxKind.NumericLiteralExpression:
                    {
                        if (context.IsRefactoringEnabled(RefactoringIdentifiers.ConvertHexadecimalLiteralToDecimalLiteral)
                            && context.Span.IsBetweenSpans(literalExpression))
                        {
                            ConvertHexadecimalLiteralToDecimalLiteralRefactoring.ComputeRefactoring(context, literalExpression);
                        }

                        break;
                    }
            }
        }
    }
}