﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Helpers;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ArgumentRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, ArgumentSyntax argument)
        {
            if (context.IsAnyRefactoringEnabled(
                RefactoringIdentifiers.AddCastExpression,
                RefactoringIdentifiers.CallToMethod))
            {
                ExpressionSyntax expression = argument.Expression;

                if (expression?.IsMissing == false)
                {
                    SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                    ITypeSymbol typeSymbol = semanticModel.GetTypeInfo(expression).ConvertedType;

                    if (typeSymbol?.IsErrorType() == false)
                    {
                        IEnumerable<ITypeSymbol> newTypes = DetermineParameterTypeHelper.DetermineParameterTypes(argument, semanticModel, context.CancellationToken)
                            .Where(f => !SymbolEqualityComparer.Default.Equals(typeSymbol, f));

                        ModifyExpressionRefactoring.ComputeRefactoring(context, expression, newTypes, semanticModel);
                    }
                }
            }
        }
    }
}
