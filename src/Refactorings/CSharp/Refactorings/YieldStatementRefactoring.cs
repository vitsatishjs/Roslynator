﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Refactorings.ConvertReturnToIf;

namespace Roslynator.CSharp.Refactorings
{
    internal static class YieldStatementRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, YieldStatementSyntax yieldStatement)
        {
            if (context.IsRefactoringEnabled(RefactoringIdentifiers.CallToMethod)
                && yieldStatement.Kind() == SyntaxKind.YieldReturnStatement)
            {
                ExpressionSyntax expression = yieldStatement.Expression;

                if (expression?.IsMissing == false
                    && expression.Span.Contains(context.Span))
                {
                    SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                    (ISymbol memberSymbol, ITypeSymbol memberTypeSymbol) = ReturnExpressionRefactoring.GetContainingSymbolAndType(expression, semanticModel, context.CancellationToken);

                    if (memberSymbol != null
                        && (memberTypeSymbol is INamedTypeSymbol namedTypeSymbol)
                        && namedTypeSymbol.SpecialType != SpecialType.System_Collections_IEnumerable
                        && namedTypeSymbol.OriginalDefinition.IsIEnumerableOfT())
                    {
                        ITypeSymbol argumentSymbol = namedTypeSymbol.TypeArguments[0];

                        ITypeSymbol expressionTypeSymbol = semanticModel.GetTypeSymbol(expression, context.CancellationToken);

                        if (!SymbolEqualityComparer.Default.Equals(argumentSymbol, expressionTypeSymbol))
                        {
                            ModifyExpressionRefactoring.ComputeRefactoring(
                                context,
                                expression,
                                argumentSymbol,
                                semanticModel,
                                addCastExpression: false);
                        }
                    }
                }
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.ConvertReturnToIf)
                && (context.Span.IsEmptyAndContainedInSpan(yieldStatement.YieldKeyword)
                    || context.Span.IsEmptyAndContainedInSpan(yieldStatement.ReturnOrBreakKeyword)
                    || context.Span.IsBetweenSpans(yieldStatement)))
            {
                await ConvertReturnToIfRefactoring.ConvertYieldReturnToIfElse.ComputeRefactoringAsync(context, yieldStatement).ConfigureAwait(false);
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.UseListInsteadOfYield)
                && yieldStatement.IsYieldReturn()
                && context.Span.IsEmptyAndContainedInSpan(yieldStatement.YieldKeyword))
            {
                SyntaxNode declaration = yieldStatement.FirstAncestor(SyntaxKind.MethodDeclaration, SyntaxKind.LocalFunctionStatement, SyntaxKind.GetAccessorDeclaration, ascendOutOfTrivia: false);

                Debug.Assert(declaration != null);

                if (declaration != null)
                {
                    SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                    UseListInsteadOfYieldRefactoring.ComputeRefactoring(context, declaration, semanticModel);
                }
            }
        }
    }
}
