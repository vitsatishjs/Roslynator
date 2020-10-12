﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Analysis.AddExceptionToDocumentationComment;
using Roslynator.CSharp.Refactorings.AddExceptionToDocumentationComment;

namespace Roslynator.CSharp.Refactorings
{
    internal static class ThrowExpressionRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, ThrowExpressionSyntax throwExpression)
        {
            if (context.IsRefactoringEnabled(RefactoringIdentifiers.AddExceptionToDocumentationComment)
                && context.Span.IsContainedInSpanOrBetweenSpans(throwExpression))
            {
                SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                AddExceptionToDocumentationCommentAnalysisResult analysis = AddExceptionToDocumentationCommentAnalysis.Analyze(
                    throwExpression,
                    semanticModel.GetTypeByMetadataName("System.Exception"),
                    semanticModel,
                    context.CancellationToken);

                if (analysis.Success)
                {
                    context.RegisterRefactoring(
                        "Add exception to documentation comment",
                        cancellationToken => AddExceptionToDocumentationCommentRefactoring.RefactorAsync(context.Document, analysis, cancellationToken),
                        RefactoringIdentifiers.AddExceptionToDocumentationComment);
                }
            }
        }
    }
}